using System.Transactions;

namespace KosHome.Infrastructure.Data.Transactions;

/// <summary>
/// Factory for creating transaction scopes.
/// </summary>
public sealed class TransactionFactory : ITransactionFactory
{
    /// <summary>
    /// Creates a new transaction scope.
    /// </summary>
    /// <param name="isolationLevel">The isolation level for the transaction.</param>
    /// <returns>A new transaction scope.</returns>
    public ITransactionScope Create(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        TransactionScopeOptions scopeOptions = new()
        {
            IsolationLevel = isolationLevel
        };

        return new TransactionScopeWrapper(scopeOptions);
    }
}
