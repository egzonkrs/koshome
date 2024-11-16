using System;
using System.Configuration;
using KosHome.Domain.Abstractions;
using KosHome.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KosHome.Api.Modules;

public sealed class DataModule : IModule
{
    private readonly IConfiguration _configuration;
    
    public DataModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void Load(IServiceCollection services)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentException(nameof(_configuration), "Cannot find 'DefaultConnection' section inside the configuration");
        
        services.AddDbContextPool<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });
    }
}