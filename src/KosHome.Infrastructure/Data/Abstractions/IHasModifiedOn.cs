using System;

namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// Interface for entities that track when they were modified.
/// </summary>
public interface IHasModifiedOn
{
    /// <summary>
    /// Modication time of this entity.
    /// </summary>
    DateTimeOffset? ModifiedOn { get; set; }
}
