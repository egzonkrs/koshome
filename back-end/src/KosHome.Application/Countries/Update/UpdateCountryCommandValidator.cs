using FluentValidation;

namespace KosHome.Application.Countries.Update;

/// <summary>
/// Validator for the <see cref="UpdateCountryCommand"/>.
/// </summary>
public sealed class UpdateCountryCommandValidator : AbstractValidator<UpdateCountryCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCountryCommandValidator"/> class.
    /// </summary>
    public UpdateCountryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Country ID is required.");

        RuleFor(x => x.CountryName)
            .NotEmpty().WithMessage("Country name is required.")
            .MaximumLength(100).WithMessage("Country name must not exceed 100 characters.");

        RuleFor(x => x.Alpha3Code)
            .NotEmpty().WithMessage("Alpha-3 code is required.")
            .Length(3).WithMessage("Alpha-3 code must be exactly 3 characters.");
    }
} 