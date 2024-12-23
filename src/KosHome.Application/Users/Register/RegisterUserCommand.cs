using System;
using FluentResults;
using MediatR;

namespace KosHome.Application.Users.Register;

/// <summary>
/// Command to register a new user.
/// </summary>
public sealed class RegisterUserCommand : IRequest<Result<Guid>>
{
    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; set; }
    
    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    public string Password { get; set; }
}