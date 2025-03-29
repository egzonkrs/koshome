using System;
using System.Collections.Generic;
using System.Linq;
using Keycloak.Net;
using FluentResults;
using System.Threading;
using System.Threading.Tasks;
using Keycloak.Net.Models.Roles;
using KosHome.Application.Abstractions.Auth.Services;
using KosHome.Domain.Entities.Users;
using KosHome.Infrastructure.Authentication.Models;
using KeycloakUser = Keycloak.Net.Models.Users.User;

namespace KosHome.Infrastructure.Authentication.Services;

/// <summary>
/// A wrapper around the <see cref="KeycloakClient"/> to provide the functionality to manage identity users of the Keycloak.
/// </summary>
public sealed class KeycloakClientWrapper : IKeycloakIdentityService
{
    private readonly KeycloakClient _keycloakClient;

    public KeycloakClientWrapper(KeycloakClient keycloakClient)
    {
        _keycloakClient = keycloakClient;
    }
    
    /// <inheritdoc />
    public async Task<Result<string>> CreateAndRetrieveUserIdAsync(string realm, KeycloakUser keycloakUser, CancellationToken cancellationToken = default)
    {
        return await _keycloakClient.CreateAndRetrieveUserIdAsync(realm, keycloakUser, cancellationToken);
    }

    public async Task<Result<IEnumerable<Role>>> GetRolesAsync(string realm, CancellationToken cancellationToken = default)
    {
        var keycloakRoles = await _keycloakClient.GetRolesAsync(realm, first: null, max: null, search: null, briefRepresentation: null, cancellationToken);
        return Result.Ok(keycloakRoles);
    }

    public async Task<Result<bool>> AddRealmRoleMappingsToUserAsync(string realm, string userId, IEnumerable<Role> roles, CancellationToken cancellationToken = default)
    {
        return await _keycloakClient.AddRealmRoleMappingsToUserAsync(realm, userId, roles, cancellationToken);
    }

    public async Task<Result<Role>> GetRoleByNameAsync(string realm, string roleName, CancellationToken cancellationToken = default)
    {
        return await _keycloakClient.GetRoleByNameAsync(realm, roleName, cancellationToken);
    }

    public async Task<IEnumerable<IdentityRole>> GetRolesAsync(string realm, int? first = null, int? max = null, string? search = null, bool? briefRepresentation = null, CancellationToken cancellationToken = default)
    {
        var roles = await _keycloakClient.GetRolesAsync(realm, first, max, search, briefRepresentation, cancellationToken);
        return roles.Select(role => new IdentityRole
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            // Attributes = role.Attributes // TODO: map the attributes
        });
    }
} 