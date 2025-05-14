using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Domain.Entities.Users;

namespace KosHome.Application.Abstractions.Auth.Services;

/// <summary>
/// A service that provides functionality to manage identity users.
/// </summary>
public interface IKeycloakIdentityService
{
    /// <summary>
    /// Registers a new identity user in the authentication system and assigns a role to the user.
    /// </summary>
    /// <param name="identityUser">The <see cref="IdentityUser"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The unique identifier of the registered user.</returns>
    Task<Result<Guid>> CreateIdentityUserAndAssignRoleAsync(IdentityUser identityUser, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Logs in a user using Keycloak and retrieves an access token.
    /// </summary>
    /// <param name="email">User's email or email.</param>
    /// <param name="password">User's password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A Result containing the token response.</returns>
    Task<Result<string>> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates a Keycloak user's attributes.
    /// </summary>
    /// <param name="identityId">The identity ID of the user.</param>
    /// <param name="attributes">The attributes to update.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A Result indicating success or failure.</returns>
    Task<Result<bool>> UpdateUserAttributesAsync(string identityId, IDictionary<string, IEnumerable<string>> attributes, CancellationToken cancellationToken = default);
}