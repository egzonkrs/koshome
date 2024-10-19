using System;
using System.Collections.Generic;
using System.Linq;

namespace KosHome.Domain.Abstractions;

/// <summary>
/// Base class for entities with domain events.
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Unique identifier of the entity.
    /// </summary>
    public Ulid Id { get; init; }

    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Domain events associated with the entity.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList();

    /// <summary>
    /// Initializes a new instance with the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    protected Entity(Ulid id)
    {
        Id = id;
    }

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    protected Entity()
    {
    }

    /// <summary>
    /// Clears all domain events.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Raises a domain event.
    /// </summary>
    /// <param name="domainEvent">The event to raise.</param>
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}

