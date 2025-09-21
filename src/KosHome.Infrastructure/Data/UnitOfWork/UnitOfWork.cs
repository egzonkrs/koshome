using System;
using System.Threading;
using System.Transactions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KosHome.Domain.Data.Abstractions;

namespace KosHome.Infrastructure.Data.UnitOfWork;

/// <summary>
/// Unit of Work implementation with transaction scope support for Ardalis repositories.
/// </summary>
internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
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

            var result = await operation(scope);
            scope.Complete();
            return result;
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
            scope.Complete();
        });
    }
}
