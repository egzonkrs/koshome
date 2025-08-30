using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KosHome.Infrastructure.Data.Abstractions;

public partial class EfCoreRepository<TPrimaryKey, TEntity>
{
    /// <summary>
    /// Executes a raw SQL query and returns entities.
    /// </summary>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">Optional parameters for the query.</param>
    /// <returns>The entities returned by the query.</returns>
    public IEnumerable<TEntity> Query(string query, object parameters = null)
    {
        return DbSet.FromSqlRaw(query, ExpandParameters(parameters).ToArray()).ToList().AsEnumerable();
    }

    /// <summary>
    /// Executes a raw SQL query and returns entities of specified type.
    /// </summary>
    /// <typeparam name="TAny">The type of entities to return.</typeparam>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">Optional parameters for the query.</param>
    /// <returns>The entities returned by the query.</returns>
    public IEnumerable<TAny> Query<TAny>(string query, object parameters = null) where TAny : class
    {
        return DbContext.Set<TAny>()
            .FromSqlRaw(query, ExpandParameters(parameters).ToArray())
            .ToList()
            .AsEnumerable();
    }

    /// <summary>
    /// Executes a raw SQL query and returns entities asynchronously.
    /// </summary>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">Optional parameters for the query.</param>
    /// <returns>A task containing the entities returned by the query.</returns>
    public Task<IEnumerable<TEntity>> QueryAsync(string query, object parameters = null)
    {
        return Task.FromResult(DbSet
            .FromSqlRaw(query, ExpandParameters(parameters).ToArray())
            .ToList()
            .AsEnumerable());
    }
}
