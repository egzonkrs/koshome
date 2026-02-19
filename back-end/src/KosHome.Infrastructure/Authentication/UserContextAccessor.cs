using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using KosHome.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace KosHome.Infrastructure.Authentication;

/// <summary>
/// Retrieves the current user’s claims from the HTTP context.
/// </summary>
public sealed class UserContextAccessor : IUserContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>All claims for the current principal (empty when no context).</summary>
    public IEnumerable<Claim> Claims => _httpContextAccessor.HttpContext?.User?.Claims ?? [];

    /// <summary>Gets the Keycloak subject / identity GUID.</summary>
    public string IdentityId => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    /// <summary>Gets the application‑level ULID of the user (or <c>Ulid.Empty</c>).</summary>
    public Ulid AppUserId
    {
        get
        {
            var hasAppUserId = Ulid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst("app_user_id")?.Value, out var convertedUlid);
            if (hasAppUserId)
            {
                return convertedUlid;
            }

            throw new InvalidOperationException($"Required claim 'app_user_id' is missing or not a valid ULID for User with Email: {Email}");
        }
    }

    /// <summary>Gets the display name.</summary>
    public string Name => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;

    /// <summary>Gets the email address.</summary>
    public string Email => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

    /// <summary>Gets the roles assigned to the current user.</summary>
    public IEnumerable<string> Roles => _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role).Select(c => c.Value) ?? [];

    /// <summary>Determines whether the current user has the specified role.</summary>
    /// <param name="role">The role to check.</param>
    /// <returns><c>true</c> if the role exists; otherwise, <c>false</c>.</returns>
    public bool HasRole(string role) => Roles.Contains(role, StringComparer.OrdinalIgnoreCase);
}