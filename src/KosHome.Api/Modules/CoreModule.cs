using KosHome.Application.Cities.GetCities;
using KosHome.Domain.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace KosHome.Api.Modules;

public sealed class CoreModule : IModule
{
    public void Load(IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCityById).Assembly));
    }
}