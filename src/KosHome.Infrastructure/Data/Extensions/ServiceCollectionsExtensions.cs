using System;
using System.Diagnostics.CodeAnalysis;
using KosHome.Domain.Data.Abstractions;
using KosHome.Infrastructure.Data.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KosHome.Infrastructure.Data.Extensions;


/// <summary>
/// The <see cref="IServiceCollection" /> Extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Entity Framework Core Unit of Work for a Context.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <typeparam name="TContext">The Db Context.</typeparam>
    /// <returns>The same <see cref="IServiceCollection" /> so that multiple calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> when the <see cref="IServiceCollection"/> is not set.</exception>
    public static IServiceCollection AddEfCoreUnitOfWork<TContext>([NotNull] this IServiceCollection services)
        where TContext : DbContext
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddScoped<IUnitOfWork<TContext>, EfUnitOfWork<TContext>>();

        return services;
    }
    
    /// <summary>
    /// Adds an Entity Framework Core Repository for an Entity.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <typeparam name="TEntity">The Entity.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <returns>The same <see cref="IServiceCollection"/> so that multiple calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> when the <see cref="IServiceCollection"/> is not set.</exception>
    public static IServiceCollection AddEfCoreRepository<TEntity, TImplementation>(
        [NotNull] this IServiceCollection services)
        where TEntity : DomainEntity, IEntity
        where TImplementation : EfRepositoryBase<TEntity>
    {
        return services.AddEfCoreRepository<TEntity, IRepository<TEntity>, TImplementation>();
    }
    
    /// <summary>
    /// Adds an Entity Framework Core Repository for an Entity.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <typeparam name="TEntity">The Entity.</typeparam>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
    /// <returns>The same <see cref="IServiceCollection"/> so that multiple calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> when the <see cref="IServiceCollection"/> is not set.</exception>
    private static IServiceCollection AddEfCoreRepository<TEntity, TService, TImplementation>(
        [NotNull] this IServiceCollection services)
        where TEntity : DomainEntity, IEntity
        where TService : class, IRepository<TEntity>
        where TImplementation : EfRepositoryBase<TEntity>, TService
    {
        ArgumentNullException.ThrowIfNull(services);

        // services.AddQueryFilters();
        services.TryAddScoped<TService, TImplementation>();

        return services;
    }
}