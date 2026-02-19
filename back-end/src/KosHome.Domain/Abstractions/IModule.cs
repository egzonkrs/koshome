using Microsoft.Extensions.DependencyInjection;

namespace KosHome.Domain.Abstractions;

/// <summary>
/// The Dependency Injection Module.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Loads the Dependency Injection Registrations.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    void Load(IServiceCollection services);
}