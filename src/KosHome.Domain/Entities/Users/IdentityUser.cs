#nullable enable
using System.Collections.Generic;

namespace KosHome.Domain.Entities.Users;

/// <summary>
/// Represents a user in the Keycloak realm.
/// </summary>
public sealed class IdentityUser
{
    /// <summary>
    /// The Keycloak user ID.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// The first name of the user.
    /// </summary>
    public required string? FirstName { get; set; }

    /// <summary>
    /// The last name of the user.
    /// </summary>
    public required string? LastName { get; set; }

    /// <summary>
    /// The username used for login.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// The user's email address.
    /// </summary>
    public required string? Email { get; set; }

    /// <summary>
    /// The user's password.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Indicates whether the user is enabled.
    /// </summary>
    public bool? IsEnabled { get; set; }

    /// <summary>
    /// Indicates whether the user's email is verified.
    /// </summary>
    public bool? IsEmailVerified { get; set; }

    /// <summary>
    /// Additional attributes stored for the user.
    /// </summary>
    public IDictionary<string, string?[]>? Attributes { get; set; }
}
