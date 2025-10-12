namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// Represents sorting criteria for queries.
/// </summary>
public interface ISort
{
    /// <summary>
    /// The property name to sort by.
    /// </summary>
    string PropertyName { get; set; }
    
    /// <summary>
    /// Indicates if sorting is ascending.
    /// </summary>
    bool IsAscending { get; set; }
}
