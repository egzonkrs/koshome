using System;
using MediatR;
using FluentResults;
using KosHome.Domain.Entities.Users;

namespace KosHome.Application.Users.Register;

/// <summary>
/// Command to create an identity user and assign a role.
/// </summary>
public sealed class RegisterUserCommand : IRequest<Result<RegisterResponse>>
{
    /// <summary>
    /// The user information to create in Keycloak.
    /// </summary>
    public IdentityUser IdentityUser { get; set; }

    /// <summary>
    /// The role name to be assigned to the created user.
    /// </summary>
    public string RoleName { get; set; }
}