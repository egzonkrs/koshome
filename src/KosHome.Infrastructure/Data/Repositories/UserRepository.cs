using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore;
using KosHome.Application.Users.Specifications;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Users;
using KosHome.Domain.ValueObjects.Users;

namespace KosHome.Infrastructure.Data.Repositories;

/// <summary>
/// User repository implementation using Ardalis.Specification.
/// </summary>
internal sealed class UserRepository : RepositoryBase<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    /// <inheritdoc />
    public async Task<User> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        var specification = new UserByEmailSpecification(email);
        return await FirstOrDefaultAsync(specification, cancellationToken);
    }
}