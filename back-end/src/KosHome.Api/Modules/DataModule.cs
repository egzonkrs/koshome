using System;
using KosHome.Domain.Abstractions;
using KosHome.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KosHome.Infrastructure.Data.Extensions;
using KosHome.Infrastructure.Images.Services;
using KosHome.Application.Abstractions.Images.Services;

namespace KosHome.Api.Modules;

/// <summary>
/// Module for configuring data access with Ardalis repositories.
/// </summary>
public sealed class DataModule : IModule
{
    private readonly IConfiguration _configuration;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DataModule"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public DataModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    /// <inheritdoc />
    public void Load(IServiceCollection services)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException(nameof(_configuration), "Cannot find 'DefaultConnection' section inside the configuration");
        
        services.AddDbContextPool<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });
        
        services.AddUnitOfWork();
        services.AddEfCoreRepositories();
        
        services.AddScoped<IApartmentImageService, ApartmentImageService>();
    }
}