namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// Interface for entities that track who created them.
/// </summary>
public interface IHasCreatedBy
{
    /// <summary>
    /// Created by user id.
    /// </summary>
    string CreatedBy { get; set; }
}
