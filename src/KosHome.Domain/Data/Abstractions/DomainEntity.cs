using System;
using System.Collections.Generic;
using System.Linq;
using KosHome.Domain.Abstractions;

namespace KosHome.Domain.Data.Abstractions;

/// <summary>
/// Base class for entities with domain events.
/// </summary>
public abstract class DomainEntity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Domain events associated with the entity.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList();

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    protected DomainEntity()
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

