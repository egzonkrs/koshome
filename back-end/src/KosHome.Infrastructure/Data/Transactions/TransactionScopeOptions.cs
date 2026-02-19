using System;
using System.Transactions;

namespace KosHome.Infrastructure.Data.Transactions;

/// <summary>
/// Options for configuring transaction scopes.
/// </summary>
public sealed class TransactionScopeOptions
{
    /// <summary>
    /// The transaction scope option.
    /// </summary>
    public TransactionScopeOption ScopeOption { get; set; }
    
    /// <summary>
    /// The isolation level for the transaction.
    /// </summary>
    public IsolationLevel IsolationLevel { get; set; }
    
    /// <summary>
    /// The async flow option for the transaction scope.
    /// </summary>
    public TransactionScopeAsyncFlowOption AsyncFlowOption { get; set; }
    
    /// <summary>
    /// The timeout for the transaction scope.
    /// </summary>
    public TimeSpan ScopeTimeout { get; set; }

    /// <summary>
    /// Initializes a new instance of the TransactionScopeOptions class with default values.
    /// </summary>
    public TransactionScopeOptions()
    {
        ScopeOption = TransactionScopeOption.Required;
        IsolationLevel = IsolationLevel.ReadCommitted;
        ScopeTimeout = TransactionManager.DefaultTimeout;
        AsyncFlowOption = TransactionScopeAsyncFlowOption.Enabled;
    }
}
