using System;
using System.Diagnostics.CodeAnalysis;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Data.Repositories;
using KosHome.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace KosHome.Infrastructure.Data.Extensions;

/// <summary>
/// The <see cref="IServiceCollection" /> Extensions for Ardalis-based repositories.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Unit of Work implementation with transaction scope support.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The same <see cref="IServiceCollection" /> so that multiple calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> when the <see cref="IServiceCollection"/> is not set.</exception>
    public static IServiceCollection AddUnitOfWork([NotNull] this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
    
    /// <summary>
    /// Adds all Ardalis-based repositories to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <returns>The same <see cref="IServiceCollection"/> so that multiple calls can be chained.</returns>
    /// <exception cref="ArgumentNullException">Throws an <see cref="ArgumentNullException"/> when the <see cref="IServiceCollection"/> is not set.</exception>
    public static IServiceCollection AddEfCoreRepositories([NotNull] this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddScoped<IApartmentRepository, ApartmentRepository>();
        services.TryAddScoped<IApartmentImageRepository, ApartmentImageRepository>();
        services.TryAddScoped<ICityRepository, CityRepository>();
        services.TryAddScoped<ICountryRepository, CountryRepository>();
        services.TryAddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
        services.TryAddScoped<IUserRepository, UserRepository>();

        return services;
    }
}