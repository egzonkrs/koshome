using System;
using System.Linq;
using KosHome.Domain.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// Entity Framework Core repository that ignores query filters.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public sealed class NoFiltersEfCoreRepository<TEntity> : NoFiltersEfCoreRepository<TEntity, Ulid>,
    IEfCoreRepository<TEntity>
    where TEntity : DomainEntity, IEntity<Ulid>
{
    /// <summary>
    /// Initializes a new instance of the NoFiltersEfCoreRepository class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public NoFiltersEfCoreRepository(DbContext dbContext) : base(dbContext)
    {
    }
}

/// <summary>
/// Entity Framework Core repository that ignores query filters.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
public class NoFiltersEfCoreRepository<TEntity, TPrimaryKey> : EfCoreRepository<TPrimaryKey, TEntity>
    where TEntity : DomainEntity, IEntity<TPrimaryKey>
{
    /// <summary>
    /// Initializes a new instance of the NoFiltersEfCoreRepository class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public NoFiltersEfCoreRepository(DbContext dbContext) : base(dbContext)
    {
    }

    /// <summary>
    /// Applies query filters to the entities. This implementation ignores all filters.
    /// </summary>
    /// <param name="entities">The entities queryable.</param>
    /// <returns>The queryable with all filters ignored.</returns>
    protected override IQueryable<TEntity> ApplyQueryFilters(IQueryable<TEntity> entities)
    {
        return entities.IgnoreQueryFilters();
    }
}
