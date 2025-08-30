using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KosHome.Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace KosHome.Infrastructure.Data.Abstractions;

public partial class EfCoreRepository<TPrimaryKey, TEntity>
{
    /// <summary>
    /// Gets the count of entities matching the optional predicate.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <returns>The count of entities.</returns>
    public int Count(Expression<Func<TEntity, bool>> predicate = null)
    {
        return ApplyQueryFilters(DbSet).AsNoTracking().CountNullable(predicate);
    }

    /// <summary>
    /// Gets the count of entities matching the optional predicate asynchronously.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <returns>A task containing the count of entities.</returns>
    public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        return ApplyQueryFilters(DbSet).AsNoTracking().CountNullableAsync(predicate);
    }

    /// <summary>
    /// Gets the long count of entities matching the optional predicate.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <returns>The long count of entities.</returns>
    public long LongCount(Expression<Func<TEntity, bool>> predicate = null)
    {
        return ApplyQueryFilters(DbSet).AsNoTracking().LongCountNullable(predicate);
    }

    /// <summary>
    /// Gets the long count of entities matching the optional predicate asynchronously.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <returns>A task containing the long count of entities.</returns>
    public Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        return ApplyQueryFilters(DbSet).AsNoTracking().LongCountNullableAsync(predicate);
    }
}
