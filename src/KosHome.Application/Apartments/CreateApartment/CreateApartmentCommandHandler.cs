using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Application.Apartments.CreateApartmentImage;
using KosHome.Domain.Abstractions;
using KosHome.Domain.Common;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Apartments;
using KosHome.Domain.Enums;
using KosHome.Domain.ValueObjects.Apartments;
using MediatR;

namespace KosHome.Application.Apartments.CreateApartment;

public sealed class CreateApartmentCommandHandler : IRequestHandler<CreateApartmentCommand, Result<Ulid>>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IPropertyTypeRepository _propertyTypeRepository;
    private readonly IUserContextAccessor _userContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISender _mediator;

    public CreateApartmentCommandHandler(
        IApartmentRepository apartmentRepository,
        IPropertyTypeRepository propertyTypeRepository,
        IUserContextAccessor userContextAccessor,
        IUnitOfWork unitOfWork,
        ISender mediator)
    {
        _apartmentRepository = apartmentRepository;
        _propertyTypeRepository = propertyTypeRepository;
        _userContextAccessor = userContextAccessor;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result<Ulid>> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
    {
        var title = new Title(request.Title);
        var description = new Description(request.Description);
        var price = new Price(request.Price);
        var address = new Address(request.Address);

        var apartment = Apartment.Create(
            _userContextAccessor.AppUserId,
            title,
            description,
            price,
            Enum.Parse<ListingType>(request.ListingType),
            Ulid.NewUlid(), // "request.PropertyType",
            address,
            request.CityId,
            request.Bedrooms,
            request.Bathrooms,
            request.SquareMeters,
            request.Latitude,
            request.Longitude);

        var apartmentId = await _apartmentRepository.InsertAsync(apartment, cancellationToken);
        var isApartmentSaved = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

        if (!isApartmentSaved)
        {
            return Result.Fail(ApartmentsErrors.NoChangesDetected());
        }

        // Process images if provided
        if (request.Images != null && request.Images.Any())
        {
            var createImagesCommand = new CreateApartmentImageCommand
            {
                ApartmentId = apartmentId,
                Images = request.Images
            };

            var imagesResult = await _mediator.Send(createImagesCommand, cancellationToken);

            if (imagesResult.IsFailed)
            {
                return Result.Fail(imagesResult.Errors);
            }
        }

        // scope.Complete();
        return Result.Ok(apartmentId);

        // return await _unitOfWork.ExecuteTransactionAsync(async scope =>
        // {
        //     
        // });
    }
    
    //
    // private bool HasImages(IEnumerable<IFormFile> images)
    // {
    //     return images.GetEnumerator().MoveNext();
    // }
}