using System;

namespace KosHome.Domain.Data.Abstractions;

/// <summary>
/// The Repository with a primary key of type Ulid.
/// </summary>
/// <typeparam name="TEntity">The domain entity type.</typeparam>
public interface IRepository<TEntity> : IRepository<Ulid, TEntity> where TEntity : DomainEntity, IEntity<Ulid>
{
}