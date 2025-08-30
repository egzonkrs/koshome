using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace KosHome.Domain.Abstractions;

/// <summary>
/// The User Context Accessor.
/// </summary>
public interface IUserContextAccessor
{
    /// <summary>
    /// The User's Claims
    /// </summary>
    IEnumerable<Claim> Claims { get; }
    
    /// <summary>
    /// The User's Keycloak Identity ID.
    /// </summary>
    string IdentityId { get; }
    
    /// <summary>
    /// The User's Application ID (ULID) in our system.
    /// </summary>
    Ulid AppUserId { get; }

    /// <summary>
    /// The User's Full Name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The User's Email.
    /// </summary>
    string Email { get; }

    /// <summary>
    /// The roles associated with the current user.
    /// </summary>
    IEnumerable<string> Roles { get; }

    /// <summary>
    /// Checks whether the current user possesses the specified role.
    /// </summary>
    /// <param name="role">The role to verify.</param>
    /// <returns><c>true</c> if the user has the role; otherwise, <c>false</c>.</returns>
    bool HasRole(string role);
}