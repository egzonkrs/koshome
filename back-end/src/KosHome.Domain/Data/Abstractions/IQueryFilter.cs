using System.Linq;

namespace KosHome.Domain.Data.Abstractions;

/// <summary>
/// The Query Filter.
/// </summary>
public interface IQueryFilter
{
    /// <summary>
    /// Applies a Query Filter for a Context.
    /// </summary>
    /// <typeparam name="TEntity">The Entity.</typeparam>
    /// <returns>The <see cref="IQueryable"/></returns>
    IQueryable<TEntity> HasQueryFilter<TEntity>(IQueryable<TEntity> queryable);
}