using System.Linq;
using System.Security.Claims;
using KosHome.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace KosHome.Infrastructure.Authentication;

/// <summary>
/// Implementation of <see cref="IUserContextAccessor"/> that retrieves user information from the current HTTP context.
/// </summary>
public sealed class UserContextAccessor : IUserContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public string Id => _httpContextAccessor.HttpContext?.User.Claims
        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

    /// <inheritdoc />
    public string Name => _httpContextAccessor.HttpContext?.User.Claims
        .FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;

    /// <inheritdoc />
    public string Email => _httpContextAccessor.HttpContext?.User.Claims
        .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
} 