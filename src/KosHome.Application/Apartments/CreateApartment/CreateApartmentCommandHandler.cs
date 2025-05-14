using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
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

    public CreateApartmentCommandHandler(
        IApartmentRepository apartmentRepository,
        IPropertyTypeRepository propertyTypeRepository,
        IUserContextAccessor userContextAccessor, IUnitOfWork unitOfWork)
    {
        _apartmentRepository = apartmentRepository;
        _propertyTypeRepository = propertyTypeRepository;
        _userContextAccessor = userContextAccessor;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Ulid>> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<ListingType>(request.ListingType, true, out var listingTypeEnum))
        {
            return Result.Fail(ApartmentsErrors.ListingTypeNotFound(request.ListingType));
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

        var qwe = _userContextAccessor.AppUserId;
        var apartment = Apartment.Create(
            _userContextAccessor.AppUserId,
            title,
            description,
            price,
            listingTypeEnum,
            propertyType.Id,
            address,
            request.CityId,
            request.Bedrooms,
            request.Bathrooms,
            request.SquareMeters,
            request.Latitude,
            request.Longitude);

        var apartmentId = await _apartmentRepository.InsertAsync(apartment, cancellationToken);
        var isApartmentSaved = await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;

        return isApartmentSaved is false 
            ? Result.Fail(ApartmentsErrors.NoChangesDetected()) 
            : Result.Ok(apartmentId);
    }
} 