using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KosHome.Infrastructure.Data.Abstractions;

public partial class EfCoreRepository<TPrimaryKey, TEntity>
{
    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    /// <summary>
    /// Updates multiple entities.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    public void Update(IEnumerable<TEntity> entities)
    {
        DbSet.UpdateRange(entities);
    }

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task representing the update operation.</returns>
    public Task UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Updates multiple entities asynchronously.
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    /// <returns>A task representing the update operation.</returns>
    public Task UpdateAsync(IEnumerable<TEntity> entities)
    {
        DbSet.UpdateRange(entities);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }
}
