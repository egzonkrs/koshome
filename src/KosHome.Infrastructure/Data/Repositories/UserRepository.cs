using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Users;
using KosHome.Domain.ValueObjects.Users;
using KosHome.Infrastructure.Data.Abstractions;

namespace KosHome.Infrastructure.Data.Repositories;


/// <summary>
/// Provides EF Core operations for City.
/// </summary>
public sealed class UserRepository : EfRepositoryBase<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public Task<User> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}