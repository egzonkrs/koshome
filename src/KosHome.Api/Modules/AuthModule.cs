using System;
using System.Threading.Tasks;
using Keycloak.Net;
using KosHome.Api.Filters;
using KosHome.Api.Handlers;
using KosHome.Application.Abstractions.Auth.Services;
using KosHome.Domain.Abstractions;
using KosHome.Infrastructure.Authentication.Abstractions;
using KosHome.Infrastructure.Authentication.Services;
using KosHome.Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using KeycloakOptions = KosHome.Infrastructure.Configurations.KeycloakOptions;

namespace KosHome.Api.Modules;

public sealed class AuthModule : IModule
{
    private readonly IConfiguration _configuration;

    public AuthModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Load(IServiceCollection services)
    {
        var authSection = _configuration.GetSection(AuthenticationOptions.SectionName);
        var keycloakSection = authSection.GetSection(KeycloakOptions.SectionName);
        var keycloakOptions = keycloakSection.Get<KeycloakOptions>();

        services.Configure<AuthenticationOptions>(authSection);
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Events.OnChallenge = CustomJwtChallengeHandler.HandleAsync;
        });

        services.AddSingleton(_ => new KeycloakClient(keycloakOptions.Authority, keycloakOptions.ClientSecret, new Keycloak.Net.KeycloakOptions(
                authenticationRealm: keycloakOptions.Realm, 
                adminClientId: keycloakOptions.ClientId)
        ));

        services
            .AddRefitClient<IKeycloakAuthApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(keycloakOptions.Authority));
        
        services.AddScoped<JwtCookieResultFilter>();
        services.AddScoped<IKeycloakClientWrapper, KeycloakClientWrapper>();
        services.AddScoped<IKeycloakIdentityService, KeycloakIdentityService>();
    }
}