using System;

namespace KosHome.Domain.Data.Abstractions;

/// <summary>
/// The Repository with Primary Key of Data Type Ulid.
/// </summary>
/// <typeparam name="TEntity">The Entity Data Type.</typeparam>
public interface IRepository<TEntity> : IRepository<Ulid, TEntity> where TEntity : DomainEntity, IEntity<Ulid>
{
}