using System;
using FluentResults;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Entities.Users;

namespace KosHome.Application.Abstractions.Auth.Services;

/// <summary>
/// A service that provides functionality to manage identity users.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Register a new identity user using the provided <paramref name="identityUser"/>.
    /// </summary>
    /// <param name="identityUser">The <see cref="IdentityUser"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task<Result<Guid>> RegisterIdentityUserAsync(IdentityUser identityUser, CancellationToken cancellationToken = default);
}