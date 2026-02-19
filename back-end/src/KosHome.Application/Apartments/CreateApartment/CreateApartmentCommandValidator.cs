using System;
using FluentValidation;
using KosHome.Domain.Enums;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Common;
using System.Threading;
using System.Threading.Tasks;

namespace KosHome.Application.Apartments.CreateApartment;

public sealed class CreateApartmentCommandValidator : AbstractValidator<CreateApartmentCommand>
{
    private readonly IPropertyTypeRepository _propertyTypeRepository;
    private static readonly CustomFluentError RequestPropertyTypeNotFound = ApartmentsErrors.RequestPropertyTypeNotFound();

    public CreateApartmentCommandValidator(IPropertyTypeRepository propertyTypeRepository)
    {
        _propertyTypeRepository = propertyTypeRepository;

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0.");

        RuleFor(x => x.ListingType)
            .NotEmpty()
            .WithMessage("Listing type is required.")
            .Must(x => Enum.TryParse<ListingType>(x, true, out _))
            .WithMessage("Invalid listing type. Valid values are: Sale, Rent");

        RuleFor(x => x.PropertyType)
            .NotEmpty()
            .WithMessage("Property type is required.")
            .MustAsync(PropertyTypeMustExistAsync)
            .WithErrorCode(RequestPropertyTypeNotFound.Code)
            .WithMessage(RequestPropertyTypeNotFound.Message);

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required.")
            .MaximumLength(200)
            .WithMessage("Address must not exceed 200 characters.");

        RuleFor(x => x.CityId)
            .NotEmpty()
            .WithMessage("City Id is required.");

        RuleFor(x => x.Bedrooms)
            .GreaterThan(0)
            .WithMessage("Number of bedrooms must be greater than 0.");

        RuleFor(x => x.Bathrooms)
            .GreaterThan(0)
            .WithMessage("Number of bathrooms must be greater than 0.");

        RuleFor(x => x.SquareMeters)
            .GreaterThan(0)
            .WithMessage("Square meters must be greater than 0.");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90.");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180.");
    }

    private async Task<bool> PropertyTypeMustExistAsync(string propertyTypeName, CancellationToken cancellationToken)
    {
        var propertyType = await _propertyTypeRepository.GetByNameAsync(propertyTypeName, cancellationToken);
        return propertyType is not null;
    }
}