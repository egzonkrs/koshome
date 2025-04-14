using KosHome.Api.Extensions.Web.Versioning;
using KosHome.Application.Cities.GetCities;
using KosHome.Domain.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using KosHome.Application.Common.Behaviors;
using MediatR;

namespace KosHome.Api.Modules;

public sealed class CoreModule : IModule
{
    public void Load(IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCityById).Assembly));
        services.AddApiExplorerVersioning();
        
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(Application.Apartments.CreateApartment.CreateApartmentCommandValidator).Assembly);
    }
}