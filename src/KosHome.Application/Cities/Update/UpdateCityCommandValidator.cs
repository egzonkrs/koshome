using FluentValidation;

namespace KosHome.Application.Cities.Update;

/// <summary>
/// Validator for the <see cref="UpdateCityCommand"/>.
/// </summary>
public sealed class UpdateCityCommandValidator : AbstractValidator<UpdateCityCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCityCommandValidator"/> class.
    /// </summary>
    public UpdateCityCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("City Id is required.");

        RuleFor(x => x.CityName)
            .NotEmpty()
            .WithMessage("City name is required.")
            .MaximumLength(50)
            .WithMessage("City name cannot exceed 50 characters.");

        RuleFor(x => x.Alpha3Code)
            .NotEmpty()
            .WithMessage("Alpha3Code is required.")
            .Length(3)
            .WithMessage("Alpha3Code must be exactly 3 characters.");

        RuleFor(x => x.CountryId)
            .NotEmpty()
            .WithMessage("Country Id is required.");
    }
} 