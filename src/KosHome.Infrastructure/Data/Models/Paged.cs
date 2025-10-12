using System.Collections.Generic;

namespace KosHome.Infrastructure.Data.Models;

/// <summary>
/// Represents a paged result set.
/// </summary>
/// <typeparam name="T">The type of entities in the page.</typeparam>
public class Paged<T>
{
    /// <summary>
    /// The current page index (zero-based).
    /// </summary>
    public int PageIndex { get; set; }
        
    /// <summary>
    /// The size of each page.
    /// </summary>
    public int PageSize { get; set; }
        
    /// <summary>
    /// The total number of records across all pages.
    /// </summary>
    public long TotalCount { get; set; }

    /// <summary>
    /// The total number of pages.
    /// </summary>
    public int TotalPages { get; set; }
        
    /// <summary>
    /// The data for the current page.
    /// </summary>
    public IEnumerable<T> Data { get; set; } = new List<T>();
}
