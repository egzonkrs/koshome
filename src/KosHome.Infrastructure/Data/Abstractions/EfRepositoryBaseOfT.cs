using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KosHome.Domain.Data.Abstractions;

namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// The EF Core Repository.
/// </summary>
/// <typeparam name="TPrimaryKey"></typeparam>
/// <typeparam name="TEntity"></typeparam>
public abstract class EfRepositoryBase<TPrimaryKey, TEntity> : IRepository<TPrimaryKey, TEntity> 
    where TEntity : DomainEntity, IEntity<TPrimaryKey>
{
    private readonly DbSet<TEntity> _dbSet;
    // private readonly IEnumerable<IQueryFilter> _queryFilters;

    /// <summary>
    /// The Constructor.
    /// </summary>
    /// <param name="dbContext">The <see cref="DbContext"/>.</param>
    protected EfRepositoryBase(DbContext dbContext)
    {
        _dbSet = dbContext.Set<TEntity>();
        // _queryFilters = dbContext.GetService<QueryFiltersAccessor>()?.QueryFilters;
    }

    // /// <inheritdoc />
    // public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification = null,
    //     CancellationToken cancellationToken = default)
    // {
    //     if (specification is null)
    //     {
    //         return await QueryFiltersInternal().ToListAsync(cancellationToken);
    //     }
    //
    //     var dbSet = QueryFiltersInternal().Where(specification); // Check QueryableExtensions
    //
    //     if (specification.PageSize.HasValue)
    //     {
    //         return dbSet.Skip((int)(specification.PageNumber * specification.PageSize.Value))
    //             .Take((int)specification.PageSize.Value);
    //     }
    //
    //     return dbSet;
    // }

    /// <inheritdoc />
    public Task<TEntity> GetByIdAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        // return QueryFiltersInternal().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    // /// <inheritdoc />
    // public async Task<bool> ExistsAsync(ISpecification<TEntity> specification = null,
    //     CancellationToken cancellationToken = default)
    // {
    //     return specification is null
    //         ? await QueryFiltersInternal().AnyAsync(cancellationToken)
    //         : await QueryFiltersInternal().Where(specification).AnyAsync(cancellationToken);
    // }

    /// <inheritdoc />
    public Task<bool> ExistsAsync(TPrimaryKey id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
        // return QueryFiltersInternal().AnyAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TPrimaryKey> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    /// <inheritdoc />
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_dbSet.Update(entity));
    }

    /// <inheritdoc />
    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_dbSet.Remove(entity));
    }

    // private IQueryable<TEntity> QueryFiltersInternal()
    // {
    //     if (_queryFilters is null) return _dbSet;
    //
    //     IQueryable<TEntity> dbSet = _dbSet;
    //
    //     foreach (var filter in _queryFilters)
    //     {
    //         dbSet = filter.HasQueryFilter(dbSet);
    //     }
    //
    //     return dbSet;
    // }
}