using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using KosHome.Domain.Data.Abstractions;
using KosHome.Infrastructure.Data.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace KosHome.Infrastructure.Data.Interop.UnitOfWork;

/// <summary>
/// Unit of work implementation with a specific database context.
/// </summary>
/// <typeparam name="TContext">The database context type.</typeparam>
public class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable where TContext : DbContext
{
    private readonly ITransactionFactory _transactionFactory;
    private readonly ILogger<UnitOfWork<TContext>> _logger;
    private readonly ConcurrentDictionary<Type, object> _repositories;
    private bool _disposed;

    /// <summary>
    /// Gets the database context.
    /// </summary>
    public TContext Context { get; }

    /// <summary>
    /// Initializes a new instance of the UnitOfWork class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="transactionFactory">The transaction factory.</param>
    /// <param name="loggerFactory">The logger factory.</param>
    public UnitOfWork(TContext dbContext, ITransactionFactory transactionFactory, ILoggerFactory loggerFactory)
    {
        Context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _transactionFactory = transactionFactory ?? throw new ArgumentNullException(nameof(transactionFactory));
        _logger = loggerFactory?.CreateLogger<UnitOfWork<TContext>>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        _repositories = new ConcurrentDictionary<Type, object>();
    }

    private async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await Context.SaveChangesAsync(cancellationToken);

    private Task DetachAllEntities()
    {
        var changedEntriesCopy = Context.ChangeTracker.Entries().ToList();

        foreach (var entry in changedEntriesCopy)
        {
            entry.State = EntityState.Detached;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(bool resetContextState = false, CancellationToken cancellationToken = default)
    {
        var count = await SaveChangesAsync(cancellationToken); 

        if (resetContextState)
        {
            await DetachAllEntities();
        }

        return count;
    }

    /// <inheritdoc />
    public ITransactionScope BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        => _transactionFactory.Create(isolationLevel);

    /// <inheritdoc />
    public TRepository GetRepository<TRepository>() where TRepository : class, IRepository
    {
        var repositoryType = typeof(TRepository);

        var repository =
            _repositories.GetOrAdd(repositoryType, v => Context.GetService<TRepository>());

        return (TRepository)repository;
    }

    /// <summary>
    /// Releases all resources used by this unit of work.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by this unit of work and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    public void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _repositories?.Clear();

                Context.Dispose();
                _logger.LogDebug($"'{nameof(UnitOfWork<TContext>)}' disposed.");
            }
        }

        _disposed = true;
    }
}
