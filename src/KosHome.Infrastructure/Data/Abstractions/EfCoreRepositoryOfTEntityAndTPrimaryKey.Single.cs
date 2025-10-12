using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KosHome.Infrastructure.Data.Abstractions;

public partial class EfCoreRepository<TPrimaryKey, TEntity>
{
    /// <summary>
    /// Gets a single entity by its primary key. Throws an exception if not found or multiple found.
    /// </summary>
    /// <param name="id">The primary key.</param>
    /// <returns>The single entity matching the key.</returns>
    public TEntity Single(TPrimaryKey id)
    {
        return ApplyQueryFilters(DbSet).AsNoTracking().Single(x => x.Id.Equals(id));
    }

    /// <summary>
    /// Gets a single entity matching the predicate. Throws an exception if not found or multiple found.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <returns>The single entity matching the predicate.</returns>
    public TEntity Single(Expression<Func<TEntity, bool>> predicate)
    {
        return ApplyQueryFilters(DbSet).AsNoTracking().Single(predicate);
    }

    /// <summary>
    /// Gets a single entity matching the predicate asynchronously. Throws an exception if not found or multiple found.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <returns>A task containing the single entity matching the predicate.</returns>
    public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return ApplyQueryFilters(DbSet).AsNoTracking().SingleAsync(predicate);
    }

    /// <summary>
    /// Gets a single entity by its primary key asynchronously. Throws an exception if not found or multiple found.
    /// </summary>
    /// <param name="id">The primary key.</param>
    /// <returns>A task containing the single entity matching the key.</returns>
    public Task<TEntity> SingleAsync(TPrimaryKey id)
    {
        return ApplyQueryFilters(DbSet).AsNoTracking().SingleAsync(x => x.Id.Equals(id));
    }
}
