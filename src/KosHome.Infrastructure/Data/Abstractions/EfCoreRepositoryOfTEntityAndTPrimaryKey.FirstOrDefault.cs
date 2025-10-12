using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KosHome.Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace KosHome.Infrastructure.Data.Abstractions;

public partial class EfCoreRepository<TPrimaryKey, TEntity>
{
    /// <summary>
    /// Gets the first entity matching the predicate or null if none found.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>A task containing the entity or null.</returns>
    public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        return FirstOrDefaultInternal(predicate, include).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Gets the first entity matching the predicate or null if none found.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>The entity or null.</returns>
    public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        return FirstOrDefaultInternal(predicate, include).FirstOrDefault();
    }

    /// <summary>
    /// Gets the first entity matching the predicate asynchronously.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <returns>A task containing the entity or null.</returns>
    public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return FirstOrDefaultInternal(predicate).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Gets the first entity matching the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <returns>The entity or null.</returns>
    public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return FirstOrDefaultInternal(predicate).FirstOrDefault();
    }

    private IQueryable<TEntity> FirstOrDefaultInternal(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        return ApplyQueryFilters(DbSet).AsNoTracking().Where(predicate).IncludeAll(include);
    }
}
