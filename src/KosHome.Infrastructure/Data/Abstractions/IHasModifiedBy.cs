namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// Interface for entities that track who modified them.
/// </summary>
public interface IHasModifiedBy
{
    /// <summary>
    /// Modified by user id.
    /// </summary>
    string ModifiedBy { get; set; }
}
