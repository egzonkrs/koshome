using System;

namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// Interface for entities that track when they were created.
/// </summary>
public interface IHasCreatedOn
{
    /// <summary>
    /// Creation time of this entity.
    /// </summary>
    DateTimeOffset CreatedOn { get; set; }
}
