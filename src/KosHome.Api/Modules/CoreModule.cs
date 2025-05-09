using KosHome.Api.Extensions.Web.Versioning;
using KosHome.Application.Cities.GetCities;
using KosHome.Domain.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using KosHome.Application.Cities.Create;
using KosHome.Application.Common.Behaviors;

namespace KosHome.Api.Modules;

public sealed class CoreModule : IModule
{
    public void Load(IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(GetCityById).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });
        
        services.AddApiExplorerVersioning();
        services.AddValidatorsFromAssembly(typeof(CreateCityCommandValidator).Assembly);
    }
}