using System.Transactions;

namespace KosHome.Infrastructure.Data.Transactions;

/// <summary>
/// Factory for creating transaction scopes.
/// </summary>
public interface ITransactionFactory
{
    /// <summary>
    /// Creates a new transaction scope.
    /// </summary>
    /// <param name="isolationLevel">The isolation level for the transaction.</param>
    /// <returns>A new transaction scope.</returns>
    ITransactionScope Create(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
}
