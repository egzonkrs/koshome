namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// Interface for entities that track who deleted them.
/// </summary>
public interface IHasDeletedBy
{
    /// <summary>
    /// Deleted by user id.
    /// </summary>
    string DeletedBy { get; set; }
}
