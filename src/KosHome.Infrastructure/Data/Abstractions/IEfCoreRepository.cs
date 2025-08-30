using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KosHome.Domain.Data.Abstractions;
using KosHome.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore.Query;

namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// Entity Framework Core repository interface with Ulid primary key.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public interface IEfCoreRepository<TEntity> : IEfCoreRepository<TEntity, Ulid>, IRepository<TEntity>
    where TEntity : DomainEntity, IEntity<Ulid>
{
}

/// <summary>
/// Entity Framework Core repository interface.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
public interface IEfCoreRepository<TEntity, TPrimaryKey> : IRepository<TPrimaryKey, TEntity>
    where TEntity : DomainEntity, IEntity<TPrimaryKey>
{
    /// <summary>
    /// Creates a repository that ignores query filters.
    /// </summary>
    /// <returns>A repository instance that ignores query filters.</returns>
    IEfCoreRepository<TEntity, TPrimaryKey> IgnoreQueryFilters();

    /// <summary>
    /// Gets the first entity matching the predicate or null if none found.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>A task containing the entity or null.</returns>
    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

    /// <summary>
    /// Gets the first entity matching the predicate or null if none found.
    /// </summary>
    /// <param name="predicate">The predicate to match.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>The entity or null.</returns>
    TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

    /// <summary>
    /// Gets all entities matching the optional predicate.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>The entities matching the criteria.</returns>
    IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

    /// <summary>
    /// Gets all entities matching the optional predicate asynchronously.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>A task containing the entities matching the criteria.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

    /// <summary>
    /// Gets a paged result of entities matching the optional criteria.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <param name="sort">Optional sort criteria.</param>
    /// <param name="pageIndex">The page index (zero-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>A paged result of entities.</returns>
    EntityPage<TEntity> GetAllPaged(Expression<Func<TEntity, bool>> predicate = null, IList<ISort> sort = null,
        int pageIndex = 0, int pageSize = 10,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        
    /// <summary>
    /// Gets a paged result of entities matching the optional criteria asynchronously.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <param name="sort">Optional sort criteria.</param>
    /// <param name="pageIndex">The page index (zero-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>A task containing a paged result of entities.</returns>
    Task<EntityPage<TEntity>> GetAllPagedAsync(Expression<Func<TEntity, bool>> predicate = null,
        IList<ISort> sort = null, int pageIndex = 0, int pageSize = 10,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

    /// <summary>
    /// Deletes entities matching the predicate.
    /// </summary>
    /// <param name="predicate">The predicate to match entities for deletion.</param>
    /// <param name="include">Optional include function for related data.</param>
    void Delete(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include);
        
    /// <summary>
    /// Deletes entities matching the predicate asynchronously.
    /// </summary>
    /// <param name="predicate">The predicate to match entities for deletion.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>A task representing the delete operation.</returns>
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include);
}
