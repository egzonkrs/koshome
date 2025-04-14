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
    private readonly IUserContextAccessor _userContextAccessor;

    public CreateApartmentCommandHandler(
        IApartmentRepository apartmentRepository,
        IUserContextAccessor userContextAccessor)
    {
        _apartmentRepository = apartmentRepository;
        _userContextAccessor = userContextAccessor;
    }

    public async Task<Result<Ulid>> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userContextAccessor.Id))
        {
            return Result.Fail(new CustomFluentError("UNAUTHORIZED_ACCESS", "User not authenticated."));
        }

        var title = new Title(request.Title);
        var description = new Description(request.Description);
        var price = new Price(request.Price);
        var address = new Address(request.Address);

        if (!Enum.TryParse<ListingType>(request.ListingType, true, out var listingType))
        {
            return Result.Fail(new CustomFluentError("INVALID_LISTING_TYPE", "Invalid listing type provided."));
        }

        if (!Enum.TryParse<PropertyType>(request.PropertyType, true, out var propertyType))
        {
            return Result.Fail(new CustomFluentError("INVALID_PROPERTY_TYPE", "Invalid property type provided."));
        }

        var apartment = Apartment.Create(
            Ulid.Parse(_userContextAccessor.Id),
            title,
            description,
            price,
            listingType,
            propertyType,
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