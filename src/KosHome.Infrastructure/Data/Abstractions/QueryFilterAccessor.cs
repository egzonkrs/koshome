using System.Collections.Generic;
using KosHome.Domain.Data.Abstractions;

namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// Query Filters Accessor.
/// </summary>
public sealed class QueryFiltersAccessor
{
    /// <summary>
    /// The Query Filters.
    /// </summary>
    public readonly IEnumerable<IQueryFilter> QueryFilters;

    /// <summary>
    /// Constructor.
    /// </summary>
    public QueryFiltersAccessor(IEnumerable<IQueryFilter> queryFilters)
    {
        QueryFilters = queryFilters;
    }
}