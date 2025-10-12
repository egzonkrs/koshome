using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KosHome.Infrastructure.Data.Abstractions;

public partial class EfCoreRepository<TPrimaryKey, TEntity>
{
    /// <summary>
    /// Checks if any entity matches the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to check.</param>
    /// <returns>True if any entity matches, false otherwise.</returns>
    public bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        return ApplyQueryFilters(DbSet).AsNoTracking().Where(predicate).Any();
    }

    /// <summary>
    /// Checks if any entity matches the predicate asynchronously.
    /// </summary>
    /// <param name="predicate">The predicate to check.</param>
    /// <returns>A task containing true if any entity matches, false otherwise.</returns>
    public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return ApplyQueryFilters(DbSet).AsNoTracking().Where(predicate).AnyAsync();
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(ISpecification<TEntity> specification = null, CancellationToken cancellationToken = default)
    {
        if (specification is null)
        {
            return await ApplyQueryFilters(DbSet).AnyAsync(cancellationToken);
        }

        // TODO: Implement specification filtering when needed
        return await ApplyQueryFilters(DbSet).AnyAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<bool> ExistsAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
    {
        return ApplyQueryFilters(DbSet).AnyAsync(x => x.Id.Equals(id), cancellationToken);
    }
}
