using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Infrastructure.Data.Extensions;
using Microsoft.EntityFrameworkCore.Query;

namespace KosHome.Infrastructure.Data.Abstractions;

public partial class EfCoreRepository<TPrimaryKey, TEntity>
{
    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public void Delete(TEntity entity)
    {
        DbContext.Remove(entity);
    }

    /// <summary>
    /// Deletes multiple entities.
    /// </summary>
    /// <param name="entities">The entities to delete.</param>
    public void Delete(IEnumerable<TEntity> entities)
    {
        DbContext.RemoveRange(entities);
    }

    /// <summary>
    /// Deletes entities matching the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to match entities for deletion.</param>
    public void Delete(Expression<Func<TEntity, bool>> predicate)
    {
        Delete(predicate, null);
    }

    /// <summary>
    /// Deletes entities matching the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to match entities for deletion.</param>
    /// <param name="include">Optional include function for related data.</param>
    public void Delete(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
    {
        DbSet.RemoveRange(GetAllToDelete(predicate, include));
    }

    /// <summary>
    /// Deletes an entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task representing the delete operation.</returns>
    public Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Deletes multiple entities asynchronously.
    /// </summary>
    /// <param name="entities">The entities to delete.</param>
    /// <returns>A task representing the delete operation.</returns>
    public Task DeleteAsync(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Deletes entities matching the predicate asynchronously.
    /// </summary>
    /// <param name="predicate">The predicate to match entities for deletion.</param>
    /// <returns>A task representing the delete operation.</returns>
    public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return DeleteAsync(predicate, null);
    }

    /// <summary>
    /// Deletes entities matching the predicate asynchronously.
    /// </summary>
    /// <param name="predicate">The predicate to match entities for deletion.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>A task representing the delete operation.</returns>
    public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
    {
        DbSet.RemoveRange(GetAllToDelete(predicate, include));
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }

    private IQueryable<TEntity> GetAllToDelete(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
    {
        return ApplyQueryFilters(DbSet)
            .WhereNullable(predicate)
            .IncludeAll(include);
    }
}
