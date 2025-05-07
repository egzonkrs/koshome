using System;
using FluentValidation;
using KosHome.Domain.Enums;

namespace KosHome.Application.Apartments.CreateApartment;

public sealed class CreateApartmentCommandValidator : AbstractValidator<CreateApartmentCommand>
{
    public CreateApartmentCommandValidator()
    {
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
            .WithMessage("Property type is required.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required.")
            .MaximumLength(200)
            .WithMessage("Address must not exceed 200 characters.");

        RuleFor(x => x.LocationId)
            .NotEmpty()
            .WithMessage("Location ID is required.");

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
} 