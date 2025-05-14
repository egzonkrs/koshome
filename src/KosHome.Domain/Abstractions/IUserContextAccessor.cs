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
} 