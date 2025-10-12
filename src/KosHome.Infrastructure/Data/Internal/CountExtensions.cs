using System;

namespace KosHome.Infrastructure.Data.Internal;

/// <summary>
/// Extension methods for count operations.
/// </summary>
internal static class CountExtensions
{
    /// <summary>
    /// Calculates the total number of pages based on count and page size.
    /// </summary>
    /// <param name="count">The total number of records.</param>
    /// <param name="pageSize">The size of each page.</param>
    /// <returns>The total number of pages.</returns>
    public static int GetTotalPages(long count, int pageSize)
    {
        if (pageSize == 0)
        {
            return 0;
        }
            
        return (int) Math.Ceiling((decimal) count / pageSize);
    }
}
