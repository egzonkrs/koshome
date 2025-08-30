using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Abstractions;
using KosHome.Infrastructure.Data.Extensions;
using KosHome.Infrastructure.Data.Internal;
using KosHome.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace KosHome.Infrastructure.Data.Abstractions;

public partial class EfCoreRepository<TPrimaryKey, TEntity>
{
    /// <summary>
    /// Gets an entity by its primary key.
    /// </summary>
    /// <param name="id">The primary key.</param>
    /// <returns>The entity or null if not found.</returns>
    public TEntity Get(TPrimaryKey id)
    {
        return ApplyQueryFilters(DbSet)
            .AsNoTracking()
            .FirstOrDefault(x => x.Id.Equals(id));
    }

    /// <summary>
    /// Gets an entity by its primary key asynchronously.
    /// </summary>
    /// <param name="id">The primary key.</param>
    /// <returns>A task containing the entity or null if not found.</returns>
    public Task<TEntity> GetAsync(TPrimaryKey id)
    {
        return ApplyQueryFilters(DbSet)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    /// <inheritdoc />
    public Task<TEntity> GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
    {
        return ApplyQueryFilters(DbSet)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    /// <summary>
    /// Gets all entities matching the optional predicate.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>The entities matching the criteria.</returns>
    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, 
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        return ApplyQueryFilters(DbSet)
            .AsNoTracking()
            .WhereNullable(predicate)
            .IncludeAll(include)
            .ToList();
    }

    /// <summary>
    /// Gets all entities matching the optional predicate.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <returns>The entities matching the criteria.</returns>
    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
    {
        return GetAll(predicate, null);
    }

    /// <summary>
    /// Gets all entities matching the optional predicate asynchronously.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <returns>A task containing the entities matching the criteria.</returns>
    public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        return GetAllAsync(predicate, null);
    }

    /// <summary>
    /// Gets all entities matching the optional predicate asynchronously.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>A task containing the entities matching the criteria.</returns>
    public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        return Task.FromResult(GetAll(predicate, include));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification = null, CancellationToken cancellationToken = default)
    {
        if (specification is null)
        {
            return await ApplyQueryFilters(DbSet).AsNoTracking().ToListAsync(cancellationToken);
        }

        IQueryable<TEntity> query = ApplyQueryFilters(DbSet).AsNoTracking();

        // Apply specification logic if needed
        // Note: For now, just return all as the specification pattern is not fully implemented
        // TODO: Implement specification filtering when needed
        
        if (specification.PageSize.HasValue)
        {
            query = query
                .Skip((int)(specification.PageNumber * specification.PageSize.Value))
                .Take((int)specification.PageSize.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets a paged result of entities matching the optional criteria.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <param name="sort">Optional sort criteria.</param>
    /// <param name="pageIndex">The page index (zero-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>A paged result of entities.</returns>
    public EntityPage<TEntity> GetAllPaged(Expression<Func<TEntity, bool>> predicate = null,
        IList<ISort> sort = null, int pageIndex = 0, int pageSize = 10,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var count = ApplyQueryFilters(DbSet).LongCountNullable(predicate);

        return GetAllPagedInternal(predicate, sort, pageIndex, pageSize, count, include);
    }

    /// <summary>
    /// Gets a paged result of entities matching the optional criteria asynchronously.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <param name="sort">Optional sort criteria.</param>
    /// <param name="pageIndex">The page index (zero-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="include">Optional include function for related data.</param>
    /// <returns>A task containing a paged result of entities.</returns>
    public async Task<EntityPage<TEntity>> GetAllPagedAsync(Expression<Func<TEntity, bool>> predicate = null,
        IList<ISort> sort = null, int pageIndex = 0, int pageSize = 10,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
    {
        var count = await ApplyQueryFilters(DbSet).LongCountNullableAsync(predicate);

        return GetAllPagedInternal(predicate, sort, pageIndex, pageSize, count, include);
    }

    /// <summary>
    /// Gets a paged result of entities matching the optional criteria.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <param name="sort">Optional sort criteria.</param>
    /// <param name="pageIndex">The page index (zero-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>A paged result of entities.</returns>
    public EntityPage<TEntity> GetAllPaged(Expression<Func<TEntity, bool>> predicate = null,
        IList<ISort> sort = null, int pageIndex = 0, int pageSize = 10)
    {
        return GetAllPaged(predicate, sort, pageIndex, pageSize, null);
    }

    /// <summary>
    /// Gets a paged result of entities matching the optional criteria asynchronously.
    /// </summary>
    /// <param name="predicate">Optional predicate to filter entities.</param>
    /// <param name="sort">Optional sort criteria.</param>
    /// <param name="pageIndex">The page index (zero-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>A task containing a paged result of entities.</returns>
    public Task<EntityPage<TEntity>> GetAllPagedAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        IList<ISort> sort = null, int pageIndex = 0, int pageSize = 10)
    {
        return GetAllPagedAsync(predicate, sort, pageIndex, pageSize, null);
    }

    private EntityPage<TEntity> GetAllPagedInternal(Expression<Func<TEntity, bool>> predicate, IList<ISort> sort,
        int pageIndex, int pageSize, long count, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
    {
        var pageCount = CountExtensions.GetTotalPages(count, pageSize);
        
        var data = ApplyQueryFilters(DbSet)
            .AsNoTracking()
            .WhereNullable(predicate)
            .IncludeAll(include)
            .Order(GetSortParameters(sort))
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToList();

        return new EntityPage<TEntity>
        {
            Data = data,
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalPages = pageCount,
            TotalCount = count
        };
    }

    /// <summary>
    /// Defaults sorting to the Id property if no sort was specified.
    /// </summary>
    /// <param name="sort">The sort criteria.</param>
    /// <returns>The sort parameters with Id as default.</returns>
    private static IList<ISort> GetSortParameters(IList<ISort> sort = null)
    {
        return sort?.Any() ?? false ? sort : new List<ISort> { new Sort { PropertyName = "Id", IsAscending = true}};
    }
}
