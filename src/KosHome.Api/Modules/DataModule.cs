using System;
using KosHome.Domain.Abstractions;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Cities;
using KosHome.Domain.Entities.Countries;
using KosHome.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using KosHome.Infrastructure.Data.Extensions;
using KosHome.Infrastructure.Data.Repositories;

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
            ?? throw new ArgumentNullException(nameof(_configuration), "Cannot find 'DefaultConnection' section inside the configuration");
        
        services.AddDbContextPool<ApplicationDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
        });
        
        services.AddEfCoreUnitOfWork<ApplicationDbContext>();
        services.AddEfCoreRepository<City, ICityRepository, CityRepository>();
        services.AddEfCoreRepository<Country, ICountryRepository, CountryRepository>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IApartmentRepository, ApartmentRepository>();
        services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
    }
}