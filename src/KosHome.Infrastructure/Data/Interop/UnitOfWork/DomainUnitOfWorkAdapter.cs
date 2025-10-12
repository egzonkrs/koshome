using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace KosHome.Infrastructure.Data.Interop.UnitOfWork;

/// <summary>
/// Adapter that bridges the Infrastructure UnitOfWork to the Domain UnitOfWork interface.
/// </summary>
/// <typeparam name="TContext">The database context type.</typeparam>
internal sealed class DomainUnitOfWorkAdapter<TContext> : Domain.Data.Abstractions.IUnitOfWork
    where TContext : DbContext
{
    private readonly IUnitOfWork<TContext> _infrastructureUnitOfWork;

    /// <summary>
    /// Initializes a new instance of the DomainUnitOfWorkAdapter class.
    /// </summary>
    /// <param name="infrastructureUnitOfWork">The infrastructure unit of work.</param>
    public DomainUnitOfWorkAdapter(IUnitOfWork<TContext> infrastructureUnitOfWork)
    {
        _infrastructureUnitOfWork = infrastructureUnitOfWork;
    }

    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _infrastructureUnitOfWork.SaveChangesAsync(false, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<T> ExecuteTransactionAsync<T>(Func<TransactionScope, Task<T>> operation, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        using var transactionScope = new TransactionScope(TransactionScopeOption.Required, 
            new TransactionOptions { IsolationLevel = isolationLevel }, 
            TransactionScopeAsyncFlowOption.Enabled);
        
        try
        {
            var result = await operation(transactionScope);
            transactionScope.Complete();
            return result;
        }
        catch
        {
            // Transaction will be automatically rolled back when disposed
            throw;
        }
    }

    /// <inheritdoc />
    public async Task ExecuteTransactionAsync(Func<TransactionScope, Task> operation, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        using var transactionScope = new TransactionScope(TransactionScopeOption.Required, 
            new TransactionOptions { IsolationLevel = isolationLevel }, 
            TransactionScopeAsyncFlowOption.Enabled);
        
        try
        {
            await operation(transactionScope);
            transactionScope.Complete();
        }
        catch
        {
            // Transaction will be automatically rolled back when disposed
            throw;
        }
    }
}
