using System;

namespace KosHome.Domain.Data.Abstractions;

/// <summary>
/// The Repository with Primary Key of Data Type long.
/// </summary>
/// <typeparam name="TEntity">The DomainEntity Data Type.</typeparam>
public interface IRepository<TEntity> : IRepository<Ulid, TEntity> where TEntity : DomainEntity
{
}