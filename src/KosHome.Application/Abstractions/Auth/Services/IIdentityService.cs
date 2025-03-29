using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Domain.Entities.Users;

namespace KosHome.Application.Abstractions.Auth.Services;

/// <summary>
/// A service that provides functionality to manage identity users.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Registers a new identity user in the authentication system and assigns a role to the user.
    /// </summary>
    /// <param name="identityUser">The <see cref="IdentityUser"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The unique identifier of the registered user.</returns>
    Task<Result<Guid>> CreateIdentityUserAndAssignRoleAsync(IdentityUser identityUser, CancellationToken cancellationToken = default);
    
    // Task<Result<TokenResponse>> LoginAsync(string realm, string username, string password, CancellationToken cancellationToken = default);
}