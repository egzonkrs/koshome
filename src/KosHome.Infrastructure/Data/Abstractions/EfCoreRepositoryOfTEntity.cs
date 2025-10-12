using System;
using KosHome.Domain.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// Entity Framework Core repository with Ulid primary key.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class EfCoreRepository<TEntity> : EfCoreRepository<Ulid, TEntity>, IEfCoreRepository<TEntity> 
    where TEntity : DomainEntity, IEntity<Ulid>
{
    /// <summary>
    /// Initializes a new instance of the EfCoreRepository class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    protected EfCoreRepository(DbContext dbContext) : base(dbContext)
    {
    }
}
