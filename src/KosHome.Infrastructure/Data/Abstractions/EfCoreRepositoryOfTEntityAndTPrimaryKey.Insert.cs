using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KosHome.Infrastructure.Data.Abstractions;

public partial class EfCoreRepository<TPrimaryKey, TEntity>
{
    /// <summary>
    /// Inserts a new entity.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    public void Insert(TEntity entity)
    {
        DbSet.Add(entity);
    }

    /// <summary>
    /// Inserts multiple entities.
    /// </summary>
    /// <param name="entities">The entities to insert.</param>
    public void Insert(IEnumerable<TEntity> entities)
    {
        DbSet.AddRange(entities);
    }

    /// <summary>
    /// Inserts a new entity and returns its ID.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>The ID of the inserted entity.</returns>
    public TPrimaryKey InsertAndGetId(TEntity entity)
    {
        DbSet.Add(entity);
        return entity.Id;
    }

    /// <summary>
    /// Inserts a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>A task representing the insert operation.</returns>
    public async Task InsertAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    /// <summary>
    /// Inserts multiple entities asynchronously.
    /// </summary>
    /// <param name="entities">The entities to insert.</param>
    /// <returns>A task representing the insert operation.</returns>
    public async Task InsertAsync(IEnumerable<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    /// <summary>
    /// Inserts a new entity and returns its ID asynchronously.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>A task containing the ID of the inserted entity.</returns>
    public async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        return entity.Id;
    }

    /// <inheritdoc />
    public async Task<TPrimaryKey> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}
