using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Domain.Entities.Users;

namespace KosHome.Application.Abstractions.Auth.Services;

/// <summary>
/// 
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="identityUser"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<Guid>> RegisterIdentityUserAsync(IdentityUser identityUser, CancellationToken cancellationToken = default);
}