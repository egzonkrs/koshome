using System;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Users;
using KosHome.Domain.ValueObjects.Users;
using KosHome.Infrastructure.Data.Abstractions;

namespace KosHome.Infrastructure.Data.Repositories;

/// <summary>
/// Provides EF Core operations for User.
/// </summary>
public sealed class UserRepository : EfCoreRepository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the UserRepository class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    /// <summary>
    /// Gets a user by email address.
    /// </summary>
    /// <param name="email">The user email.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The user with the specified email.</returns>
    public Task<User> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}