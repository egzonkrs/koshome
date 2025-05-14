using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Domain.Abstractions;
using KosHome.Domain.Common;
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

    public CreateApartmentCommandHandler(
        IApartmentRepository apartmentRepository,
        IPropertyTypeRepository propertyTypeRepository,
        IUserContextAccessor userContextAccessor)
    {
        _apartmentRepository = apartmentRepository;
        _propertyTypeRepository = propertyTypeRepository;
        _userContextAccessor = userContextAccessor;
    }

    public async Task<Result<Ulid>> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
    {
        // if (string.IsNullOrEmpty(_userContextAccessor.Id))
        // {
        //     return Result.Fail(ApartmentsErrors.UnauthorizedAccess());
        // }

        // Parse ListingType enum from request
        if (!Enum.TryParse<ListingType>(request.ListingType, true, out var listingTypeEnum))
        {
            return Result.Fail(new CustomFluentError("INVALID_LISTING_TYPE", $"Invalid listing type value: '{request.ListingType}'. Valid values are Sale, Rent."));
        }

        // Get property type by name
        var propertyType = await _propertyTypeRepository.GetByNameAsync(request.PropertyType, cancellationToken);
        if (propertyType is null)
        {
            return Result.Fail(ApartmentsErrors.PropertyTypeNotFound(request.PropertyType));
        }

        var title = new Title(request.Title);
        var description = new Description(request.Description);
        var price = new Price(request.Price);
        var address = new Address(request.Address);

        var apartment = Apartment.Create(
            Ulid.NewUlid(),
            // Ulid.Parse(_userContextAccessor.Id),
            title,
            description,
            price,
            listingTypeEnum,
            propertyType.Id,
            address,
            request.LocationId,
            request.Bedrooms,
            request.Bathrooms,
            request.SquareMeters,
            request.Latitude,
            request.Longitude);

        var apartmentId = await _apartmentRepository.InsertAsync(apartment, cancellationToken);
        return Result.Ok(apartmentId);
    }
} 