using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Application.Abstractions.Images.Services;
using KosHome.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace KosHome.Infrastructure.Images.Services;

/// <summary>
/// Service for processing and saving apartment images.
/// </summary>
public sealed class ApartmentImageService : IApartmentImageService
{
    private readonly ILogger<ApartmentImageService> _logger;
    private const string ApartmentImagesDirectory = "wwwroot/images/apartments";
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];
    private const int MaxFileSize = 10 * 1024 * 1024; // 10 MB
    private const int MaxWidth = 1920;
    private const int MaxHeight = 1080;
    private const double TargetAspectRatio = 16d / 9d; // Standard 16:9 aspect ratio
    private const int DefaultQuality = 85; // JPEG quality for compression

    public ApartmentImageService(ILogger<ApartmentImageService> logger)
    {
        _logger = logger;
    }

    public async Task<Result<string>> ProcessApartmentImageAsync(Ulid apartmentId, IFormFile image, bool isPrimary, CancellationToken cancellationToken)
    {
        try
        {
            if (image is null || image.Length is 0)
            {
                _logger.LogError("Apartment image upload failed: no file was provided.");
                return Result.Fail(ApartmentImagesErrors.NoImageProvided());
            }

            // Enforce max file size
            if (image.Length > MaxFileSize)
            {
                const int sizeInMb = MaxFileSize / (1024 * 1024);
                _logger.LogError("Apartment image upload failed: file size {FileSize} exceeds {MaxFileSize}.", image.Length, MaxFileSize);
                return Result.Fail(ApartmentImagesErrors.ImageSizeTooLarge(sizeInMb));
            }

            var fileExtension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(fileExtension))
            {
                var allowedTypes = string.Join(", ", _allowedExtensions);
                _logger.LogError("Apartment image upload failed: file extension {FileExtension} is not allowed.", fileExtension);
                return Result.Fail(ApartmentImagesErrors.UnsupportedImageFormat(allowedTypes));
            }

            // Create directory if it doesn't exist
            if (!Directory.Exists(ApartmentImagesDirectory))
            {
                Directory.CreateDirectory(ApartmentImagesDirectory);
            }

            // Generate the file name with pattern: ApartmentId_RandomUlid.extension
            var randomUlid = Ulid.NewUlid();
            var fileName = $"{apartmentId}_{randomUlid}{fileExtension}";
            var fullPath = Path.Combine(ApartmentImagesDirectory, fileName);

            _logger.LogInformation("Processing apartment image: {FileName}", fileName);

            // Process and save the image
            using (var imageStream = image.OpenReadStream())
            using (var imageProcessed = await Image.LoadAsync(imageStream, cancellationToken))
            {
                // Crop to standard aspect ratio (16:9)
                var currentRatio = (double)imageProcessed.Width / imageProcessed.Height;
                if (Math.Abs(currentRatio - TargetAspectRatio) > 0.01)
                {
                    if (currentRatio > TargetAspectRatio)
                    {
                        var newWidth = (int)(imageProcessed.Height * TargetAspectRatio);
                        var startX = (imageProcessed.Width - newWidth) / 2;
                        var cropRect = new Rectangle(startX, 0, newWidth, imageProcessed.Height);
                        imageProcessed.Mutate(x => x.Crop(cropRect));
                    }
                    else
                    {
                        var newHeight = (int)(imageProcessed.Width / TargetAspectRatio);
                        var startY = (imageProcessed.Height - newHeight) / 2;
                        var cropRect = new Rectangle(0, startY, imageProcessed.Width, newHeight);
                        imageProcessed.Mutate(x => x.Crop(cropRect));
                    }
                }

                // Resize while maintaining aspect ratio
                var resizeOptions = new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(MaxWidth, MaxHeight)
                };

                imageProcessed.Mutate(x => x.Resize(resizeOptions));

                // Set JPEG compression options if it's a JPEG
                if (fileExtension == ".jpg" || fileExtension == ".jpeg")
                {
                    await imageProcessed.SaveAsync(fullPath, new JpegEncoder 
                    { 
                        Quality = DefaultQuality 
                    }, cancellationToken);
                }
                else
                {
                    await imageProcessed.SaveAsync(fullPath, cancellationToken);
                }
            }

            _logger.LogInformation("Apartment image processed and saved successfully: {FilePath}", fileName);
            return Result.Ok(fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing apartment image for apartment {ApartmentId}.", apartmentId);
            return Result.Fail(ApartmentImagesErrors.ImageProcessingError());
        }
    }

    public async Task<Result<List<string>>> ProcessApartmentImagesAsync(Ulid apartmentId, IEnumerable<IFormFile> images, CancellationToken cancellationToken)
    {
        var imageList = images.ToList();
        if (imageList.Count is 0)
        {
            return Result.Fail(ApartmentImagesErrors.NoImageProvided());
        }

        var results = new List<string>();
        var errors = new List<IError>();

        for (var i = 0; i < imageList.Count; i++)
        {
            var isPrimary = i is 0; // First image is always primary
            var result = await ProcessApartmentImageAsync(apartmentId, imageList[i], isPrimary, cancellationToken);
            
            if (result.IsSuccess)
            {
                results.Add(result.Value);
            }
            else
            {
                errors.AddRange(result.Errors);
            }
        }

        return errors.Any() 
            ? Result.Fail(errors) 
            : Result.Ok(results);
    }
} 