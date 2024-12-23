using System.Threading;
using System.Threading.Tasks;

namespace KosHome.Domain.Data.Abstractions;

public interface IUnitOfWork
{
    /// <summary>
    /// Asynchronously saves all changes made in the context of the current unit of work.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous save operation. The task result contains
    /// the number of state entries written to the underlying database.
    /// </returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}