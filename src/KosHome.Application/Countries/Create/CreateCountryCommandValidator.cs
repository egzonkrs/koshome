using FluentValidation;

namespace KosHome.Application.Countries.Create;

/// <summary>
/// Validator for the <see cref="CreateCountryCommand"/>.
/// </summary>
public sealed class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCountryCommandValidator"/> class.
    /// </summary>
    public CreateCountryCommandValidator()
    {
        RuleFor(x => x.CountryName)
            .NotEmpty().WithMessage("Country name is required.")
            .MaximumLength(100).WithMessage("Country name must not exceed 100 characters.");

        RuleFor(x => x.Alpha3Code)
            .NotEmpty().WithMessage("Alpha-3 code is required.")
            .Length(3).WithMessage("Alpha-3 code must be exactly 3 characters.");
    }
} 