using Microsoft.EntityFrameworkCore;

namespace KosHome.Infrastructure.Data.Interop.UnitOfWork;

/// <summary>
/// Represents a unit of work with a specific database context.
/// </summary>
/// <typeparam name="TContext">The database context type.</typeparam>
public interface IUnitOfWork<out TContext> : IUnitOfWork where TContext : DbContext
{
    /// <summary>
    /// Gets the database context.
    /// </summary>
    TContext Context { get; }
}
