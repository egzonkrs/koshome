using System;
using KosHome.Domain.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// The EF Core Repository.
/// </summary>
/// <typeparam name="TEntity">The DomainEntity Date Type.</typeparam>
public abstract class EfRepositoryBase<TEntity> : EfRepositoryBase<Ulid, TEntity>, IRepository<TEntity> where TEntity : DomainEntity, IEntity<Ulid>
{
    /// <inheritdoc/>
    protected EfRepositoryBase(DbContext dbContext) : base(dbContext)
    {
    }
}