using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using KosHome.Domain.Entities.Users;
using KosHome.Domain.ValueObjects.Users;

namespace KosHome.Domain.Data.Repositories;

/// <summary>
/// Defines database operations for User.
/// </summary>
public interface IUserRepository : IRepositoryBase<User>
{
    /// <summary>
    /// Gets a user by email.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<User> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
}