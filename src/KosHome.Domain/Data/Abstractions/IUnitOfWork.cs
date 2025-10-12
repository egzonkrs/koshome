using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace KosHome.Domain.Data.Abstractions;

/// <summary>
/// The Unit of Work for application layer usage.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Saves all the changes made in the context to the database.
    /// </summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/></param>
    /// <returns>The number of entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes an asynchronous operations inside a Transaction Scope.
    /// </summary>
    /// <typeparam name="T">The Data Type.</typeparam>
    /// <param name="operation">The Operation to execute.</param>
    /// <param name="isolationLevel">The Isolation Level. Defaults to "ReadCommitted".</param>
    Task<T> ExecuteTransactionAsync<T>(Func<TransactionScope, Task<T>> operation, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

    /// <summary>
    /// Executes an asynchronous operations inside a Transaction Scope.
    /// </summary>
    /// <param name="operation">The Operation to execute.</param>
    /// <param name="isolationLevel">The Isolation Level. Defaults to "ReadCommitted".</param>
    Task ExecuteTransactionAsync(Func<TransactionScope, Task> operation, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
}
