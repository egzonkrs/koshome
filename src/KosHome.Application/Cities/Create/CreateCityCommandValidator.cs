using FluentValidation;
using KosHome.Application.Cities.Create;

namespace KosHome.Application.Cities.Create;

public sealed class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator()
    {
        RuleFor(x => x.CityName)
            .NotEmpty()
            .WithMessage("City name is required.")
            .MaximumLength(100)
            .WithMessage("City name must not exceed 100 characters.");

        RuleFor(x => x.Alpha3Code)
            .NotEmpty()
            .WithMessage("Alpha-3 code is required.")
            .Length(3)
            .WithMessage("Alpha-3 code must be exactly 3 characters.");

        RuleFor(x => x.CountryId)
            .NotEmpty()
            .WithMessage("Country ID is required.");
    }
} 