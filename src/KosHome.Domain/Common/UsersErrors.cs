using System;
using FluentResults;

namespace KosHome.Domain.Common;

/// <summary>
/// Provides centralized error definitions related to user operations.
/// </summary>
public static class UsersErrors
{
    /// <summary>
    /// Returns an error indicating that an unexpected error occurred during a user related process.
    /// </summary>
    public static CustomFluentError UnexpectedError()
        => new CustomFluentError("USER_UNEXPECTED_ERROR","An unexpected error occurred during user creation.");
    
    /// <summary>
    /// Returns an error indicating that a user with the specified ID was not found.
    /// </summary>
    /// <param name="id">The ID of the user that was not found.</param>
    public static CustomFluentError NotFound(string id)
        => new CustomFluentError("USER_NOT_FOUND", $"User with Id: `{id}` was not found");

    /// <summary>
    /// Returns an error indicating that a role with the specified name was not found.
    /// </summary>
    /// <param name="roleName">The name of the role that was not found.</param>
    public static CustomFluentError RoleNotFound(string roleName)
        => new CustomFluentError("ROLE_NOT_FOUND", $"Role with Name: `{roleName}` was not found");

    /// <summary>
    /// Returns an error indicating that no changes were detected during the operation.
    /// </summary>
    public static CustomFluentError NoChangesDetected
        => new CustomFluentError("NO_CHANGES_DETECTED", "No changes were detected.");

    /// <summary>
    /// Returns an error indicating that the user is not authorized to perform the requested action.
    /// </summary>
    public static CustomFluentError Unauthorized
        => new CustomFluentError("UNAUTHORIZED", "User is not authorized to perform this action.");

    /// <summary>
    /// Returns an error indicating that the specified identity role is invalid.
    /// </summary>
    public static CustomFluentError IdentityRoleInvalid
        => new CustomFluentError("IDENTITY_ROLE_INVALID", "The specified identity role is invalid.");

    /// <summary>
    /// Returns an error indicating that the specified identity role was not found.
    /// </summary>
    public static CustomFluentError IdentityRoleNotFound
        => new CustomFluentError("IDENTITY_ROLE_NOT_FOUND", "The specified identity role was not found.");

    /// <summary>
    /// Returns an error indicating that the specified identity user is invalid.
    /// </summary>
    public static CustomFluentError IdentityUserInvalid => 
        new CustomFluentError("IDENTITY_USER_INVALID", "The identity user is invalid.");

    /// <summary>
    /// Returns an error indicating that the provided login credentials are invalid.
    /// </summary>
    public static CustomFluentError InvalidCredentials()
        => new CustomFluentError("INVALID_CREDENTIALS", "The username or password provided is incorrect.");
    
    /// <summary>
    /// Returns an error indicating that the specified identity user was not found.
    /// </summary>
    public static CustomFluentError IdentityUserNotFound
        => new CustomFluentError("IDENTITY_USER_NOT_FOUND", "The specified identity user was not found.");
}
