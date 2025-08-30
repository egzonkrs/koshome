using KosHome.Infrastructure.Data.Abstractions;

namespace KosHome.Infrastructure.Data.Models;

/// <summary>
/// Represents sorting criteria for queries.
/// </summary>
public sealed class Sort : ISort
{
    /// <summary>
    /// The property name to sort by.
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;
    
    /// <summary>
    /// Indicates if sorting is ascending.
    /// </summary>
    public bool IsAscending { get; set; } = true;
}
