using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using KosHome.Domain.Data.Abstractions;
using KosHome.Infrastructure.Data.Transactions;

namespace KosHome.Infrastructure.Data.Interop.UnitOfWork;

/// <summary>
/// Base Unit of Work interface.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all changes made in the context to the database asynchronously.
    /// </summary>
    /// <param name="resetContextState">Whether to reset the context state after saving.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(bool resetContextState = false, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Begins a new transaction with the specified isolation level.
    /// </summary>
    /// <param name="isolationLevel">The isolation level for the transaction.</param>
    /// <returns>A transaction scope.</returns>
    ITransactionScope BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    
    /// <summary>
    /// Gets a repository instance for the specified type.
    /// </summary>
    /// <typeparam name="TRepository">The repository type.</typeparam>
    /// <returns>The repository instance.</returns>
    TRepository GetRepository<TRepository>() where TRepository : class, IRepository;
}
