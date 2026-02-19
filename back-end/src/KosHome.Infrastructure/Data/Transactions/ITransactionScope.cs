using System;

namespace KosHome.Infrastructure.Data.Transactions;

/// <summary>
/// Represents a transaction scope that can be completed and disposed.
/// </summary>
public interface ITransactionScope : IDisposable
{
    /// <summary>
    /// Completes the transaction scope.
    /// </summary>
    void Complete();
}
