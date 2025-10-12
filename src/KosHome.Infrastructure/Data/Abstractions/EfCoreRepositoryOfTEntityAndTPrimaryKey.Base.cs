using System;
using System.Collections.Generic;
using System.Linq;
using KosHome.Domain.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// Entity Framework Core repository base implementation.
/// </summary>
/// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract partial class EfCoreRepository<TPrimaryKey, TEntity> : IEfCoreRepository<TEntity, TPrimaryKey>
    where TEntity : DomainEntity, IEntity<TPrimaryKey>
{
    private readonly DbContext DbContext;
    protected readonly DbSet<TEntity> DbSet;

    /// <summary>
    /// Initializes a new instance of the EfCoreRepository class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    protected EfCoreRepository(DbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }

    /// <summary>
    /// Creates a repository that ignores query filters.
    /// </summary>
    /// <returns>A repository instance that ignores query filters.</returns>
    public IEfCoreRepository<TEntity, TPrimaryKey> IgnoreQueryFilters()
    {
        return new NoFiltersEfCoreRepository<TEntity, TPrimaryKey>(DbContext);
    }

    /// <summary>
    /// Applies query filters to the entities. Override to customize filtering.
    /// </summary>
    /// <param name="entities">The entities queryable.</param>
    /// <returns>The filtered queryable.</returns>
    protected virtual IQueryable<TEntity> ApplyQueryFilters(IQueryable<TEntity> entities)
    {
        // Get query filters from the context if available
        if (DbContext.GetService<QueryFiltersAccessor>().QueryFilters is IEnumerable<IQueryFilter> queryFilters)
        {
            var filteredQuery = entities;
            
            foreach (var filter in queryFilters)
            {
                filteredQuery = filter.HasQueryFilter(filteredQuery);
            }
            
            return filteredQuery;
        }
        
        return entities;
    }

    /// <summary>
    /// Expands parameters for internal use.
    /// </summary>
    /// <param name="parameters">The parameters to expand.</param>
    /// <returns>The expanded parameters.</returns>
    private static IEnumerable<object> ExpandParameters(object parameters)
    {
        return parameters.GetType().IsArray 
            ? ((object[])parameters).ToList()
            : new List<object> { parameters };
    }
}
