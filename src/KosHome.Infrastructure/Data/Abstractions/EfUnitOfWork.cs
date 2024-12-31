using System;
using System.Threading;
using System.Transactions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KosHome.Domain.Data.Abstractions;

namespace KosHome.Infrastructure.Data.Abstractions;

/// <summary>
/// EF Core Unit of Work.
/// </summary>
public class EfUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _dbContext;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="dbContext">The <see cref="DbContext"/>.</param>
    public EfUnitOfWork(TContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public Task<T> ExecuteTransactionAsync<T>(Func<TransactionScope, Task<T>> operation,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        var strategy = _dbContext.Database.CreateExecutionStrategy();

        return strategy.ExecuteAsync(async () =>
        {
            using var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = isolationLevel },
                TransactionScopeAsyncFlowOption.Enabled);

            return await operation(scope);
        });
    }

    /// <inheritdoc />
    public Task ExecuteTransactionAsync(Func<TransactionScope, Task> operation,
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        var strategy = _dbContext.Database.CreateExecutionStrategy();

        return strategy.ExecuteAsync(async () =>
        {
            using var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = isolationLevel },
                TransactionScopeAsyncFlowOption.Enabled);

            await operation(scope);
        });
    }
}