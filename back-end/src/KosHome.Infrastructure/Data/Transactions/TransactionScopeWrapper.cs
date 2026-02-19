using System;
using System.Transactions;

namespace KosHome.Infrastructure.Data.Transactions;

/// <summary>
/// Wrapper for System.Transactions.TransactionScope that implements ITransactionScope.
/// </summary>
public sealed class TransactionScopeWrapper : ITransactionScope
{
    private readonly TransactionScope _transactionScope;

    /// <summary>
    /// Initializes a new instance of the TransactionScopeWrapper class.
    /// </summary>
    /// <param name="scopeOptions">The options for the transaction scope.</param>
    public TransactionScopeWrapper(TransactionScopeOptions scopeOptions)
    {
        ArgumentNullException.ThrowIfNull(scopeOptions);
        _transactionScope = GetTransactionScope(scopeOptions) ?? throw new ArgumentNullException(nameof(GetTransactionScope));
    }

    /// <summary>
    /// Completes the transaction scope.
    /// </summary>
    public void Complete()
    {
        _transactionScope.Complete();
    }

    /// <summary>
    /// Releases all resources used by the TransactionScopeWrapper.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _transactionScope?.Dispose();
        }
    }

    private static TransactionScope GetTransactionScope(TransactionScopeOptions scopeOptions)
    {
        TransactionOptions options = default;
        options.IsolationLevel = scopeOptions.IsolationLevel;
        options.Timeout = scopeOptions.ScopeTimeout;

        return new TransactionScope(scopeOptions.ScopeOption, options, scopeOptions.AsyncFlowOption);
    }
}
