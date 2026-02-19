using FluentValidation;
using KosHome.Application.Users.Register;

namespace KosHome.Application.Users.Register;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.IdentityUser.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(50)
            .WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.IdentityUser.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(50)
            .WithMessage("Last name must not exceed 50 characters.");

        RuleFor(x => x.IdentityUser.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email address format.")
            .MaximumLength(100)
            .WithMessage("Email must not exceed 100 characters.");

        RuleFor(x => x.IdentityUser.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(100)
            .WithMessage("Password must not exceed 100 characters.")
            .Matches("[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]")
            .WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]")
            .WithMessage("Password must contain at least one special character.");

        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("Role name is required.");
    }
} 