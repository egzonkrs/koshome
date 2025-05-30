using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Application.Abstractions.Images.Services;
using KosHome.Domain.Common;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.ApartmentImages;
using KosHome.Domain.ValueObjects.ApartmentImages;
using MediatR;

namespace KosHome.Application.Apartments.CreateApartmentImage;

/// <summary>
/// Handler for creating apartment images.
/// </summary>
public sealed class CreateApartmentImageCommandHandler : IRequestHandler<CreateApartmentImageCommand, Result<List<Ulid>>>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IApartmentImageRepository _apartmentImageRepository;
    private readonly IApartmentImageService _apartmentImageService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateApartmentImageCommandHandler(
        IApartmentRepository apartmentRepository,
        IApartmentImageRepository apartmentImageRepository,
        IApartmentImageService apartmentImageService,
        IUnitOfWork unitOfWork)
    {
        _apartmentRepository = apartmentRepository;
        _apartmentImageRepository = apartmentImageRepository;
        _apartmentImageService = apartmentImageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<Ulid>>> Handle(CreateApartmentImageCommand request, CancellationToken cancellationToken)
    {
        // Verify apartment exists
        var apartment = await _apartmentRepository.GetByIdAsync(request.ApartmentId, cancellationToken);
        if (apartment is null)
        {
            return Result.Fail(ApartmentImagesErrors.ApartmentNotFound(request.ApartmentId.ToString()));
        }

        // Process apartment images
        var imagesResult = await _apartmentImageService.ProcessApartmentImagesAsync(
            request.ApartmentId, request.Images, cancellationToken);

        if (imagesResult.IsFailed)
        {
            return Result.Fail(imagesResult.Errors);
        }

        var imageUrls = imagesResult.Value;
        var imageIds = new List<Ulid>();

        // Save image references to database
        for (int i = 0; i < imageUrls.Count; i++)
        {
            var isPrimary = i == 0; // First image is always primary
            var imageUrl = new ImageUrl(imageUrls[i]);
            var apartmentImage = ApartmentImage.Create(request.ApartmentId, imageUrl, isPrimary);
            
            var imageId = await _apartmentImageRepository.InsertAsync(apartmentImage, cancellationToken);
            imageIds.Add(imageId);
        }

        var changes = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (changes <= 0)
        {
            return Result.Fail(ApartmentImagesErrors.NoChangesDetected());
        }

        return Result.Ok(imageIds);
    }
} 