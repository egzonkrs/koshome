using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KosHome.Infrastructure.Data.Abstractions;

public partial class EfCoreRepository<TPrimaryKey, TEntity>
{
    /// <summary>
    /// Executes a raw SQL command.
    /// </summary>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">Optional parameters for the query.</param>
    /// <returns>The number of rows affected.</returns>
    public int Execute(string query, object parameters = null)
    {
        return DbContext.Database.ExecuteSqlRaw(query, ExpandParameters(parameters));
    }

    /// <summary>
    /// Executes a raw SQL command asynchronously.
    /// </summary>
    /// <param name="query">The SQL query to execute.</param>
    /// <param name="parameters">Optional parameters for the query.</param>
    /// <returns>A task containing the number of rows affected.</returns>
    public Task<int> ExecuteAsync(string query, object parameters = null)
    {
        return DbContext.Database.ExecuteSqlRawAsync(query, ExpandParameters(parameters));
    }
}
