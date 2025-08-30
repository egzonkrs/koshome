using System;

namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// Interface for entities that support soft deletion.
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// Used to mark an Entity as 'Deleted'. 
    /// </summary>
    bool IsDeleted { get; set; }
    
    /// <summary>
    /// Deletion time of this entity.
    /// </summary>
    DateTimeOffset? DeletedOn { get; set; }
}
