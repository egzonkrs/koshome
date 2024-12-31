using System;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Entities.Cities;
using KosHome.Domain.Entities.Users;
using KosHome.Domain.ValueObjects.Cities;
using KosHome.Domain.ValueObjects.Users;

namespace KosHome.Domain.Data.Repositories;

/// <summary>
/// Defines database operations for City.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
}