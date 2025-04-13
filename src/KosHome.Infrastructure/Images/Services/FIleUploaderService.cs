using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Application.Abstractions.Images.Services;
using KosHome.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace KosHome.Infrastructure.Images.Services;

/// <summary>
/// Service for handling file uploads using ImageSharp and FluentResults.
/// </summary>
public sealed class FileUploaderService : IFileUploaderService
{
    private readonly ILogger<FileUploaderService> _logger;

    private const string PetImagesDirectory = "wwwroot/images/pets";
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];
    private const int MaxFileSize = 10 * 1024 * 1024; // 10 MB

    public FileUploaderService(ILogger<FileUploaderService> logger)
    {
        _logger = logger;
    }

    public async Task<Result<string>> UploadFileAsync(IFormFile file)
    {
        try
        {
            if (file is null || file.Length is 0)
            {
                _logger.LogError("File upload failed: no file was provided.");
                return Result.Fail(FilesErrors.NoFileProvided);
            }

            // Enforce max file size
            if (file.Length > MaxFileSize)
            {
                const int sizeInMb = MaxFileSize / (1024 * 1024);
                _logger.LogError("File upload failed: file size {FileSize} exceeds {MaxFileSize}.", file.Length, MaxFileSize);
                
                return Result.Fail(FilesErrors.MaxFileSizeExceeded(sizeInMb));
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(fileExtension))
            {
                var allowedTypes = string.Join(", ", _allowedExtensions);
                _logger.LogError("File upload failed: file extension {FileExtension} is not allowed.", fileExtension);
                
                return Result.Fail(FilesErrors.UnsupportedFileFormat(allowedTypes));
            }

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var fullPath = Path.Combine(PetImagesDirectory, fileName);

            if (!Directory.Exists(PetImagesDirectory))
            {
                Directory.CreateDirectory(PetImagesDirectory);
            }

            _logger.LogInformation("Starting file upload: {FileName}", fileName);

            // Load the file with ImageSharp to confirm it's a valid image
            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                await image.SaveAsync(fullPath);
            }

            _logger.LogInformation("File uploaded successfully: {FilePath}", fileName);

            return Result.Ok(fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while uploading the file to {Directory}.", PetImagesDirectory);
            return Result.Fail(FilesErrors.UnexpectedError);
        }
    }
}
