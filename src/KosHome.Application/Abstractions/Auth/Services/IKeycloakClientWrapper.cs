using System.Collections.Generic;
using FluentResults;
using System.Threading;
using System.Threading.Tasks;
using Keycloak.Net.Models.Roles;
using KosHome.Domain.Entities.Users;
using KeycloakUser =  Keycloak.Net.Models.Users.User;

namespace KosHome.Application.Abstractions.Auth.Services;

/// <summary>
/// A service that provides functionality to manage identity users of the Keycloak.
/// </summary>
public interface IKeycloakClientWrapper
{
    /// <summary>
    /// Register a new identity user using the provided <paramref name="keycloakUser"/>.
    /// </summary>
    /// <param name="realm">The realm to register </param>
    /// <param name="keycloakUser">The <see cref="KeycloakUser"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The user id of the newly created user.</returns>
    Task<Result<string>> CreateAndRetrieveUserIdAsync(string realm, KeycloakUser keycloakUser, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a paginated list of roles from the specified realm.
    /// </summary>
    /// <param name="realm">The realm from which to retrieve roles.</param>
    /// <param name="first">The index of the first result to retrieve (optional).</param>
    /// <param name="max">The maximum number of roles to retrieve (optional).</param>
    /// <param name="search">A search query to filter the roles (optional).</param>
    /// <param name="briefRepresentation">A flag indicating whether to return a brief representation of the roles (optional).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A collection of roles from the specified realm.</returns>
    Task<IEnumerable<Role>> GetRolesAsync(string realm, int? first = null, int? max = null, string? search = null, bool? briefRepresentation = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds realm role mappings to a user.
    /// </summary>
    /// <param name="realm">The realm where the user exists.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="roles">The roles to add to the user.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result<bool>> AddRealmRoleMappingsToUserAsync(string realm, string userId, IEnumerable<Role> roles, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a role by name from the specified realm.
    /// </summary>
    /// <param name="realm">The realm to get the role from.</param>
    /// <param name="roleName">The name of the role to get.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>The role if found, or an error if not found.</returns>
    Task<Result<Role>> GetRoleByNameAsync(string realm, string roleName, CancellationToken cancellationToken = default);
}