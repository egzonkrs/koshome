using System;
using KosHome.Application.Abstractions.Auth.Services;
using KosHome.Domain.Abstractions;
using KosHome.Infrastructure.Authentication;
using KosHome.Infrastructure.Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        // // Configure Keycloak options from configuration and bind them to our KeycloakOptions record.
        // var keycloakConfig = _configuration.GetSection("Keycloak");
        // ArgumentNullException.ThrowIfNull(keycloakConfig, nameof(keycloakConfig));
        // services.Configure<KeycloakOptions>(keycloakConfig);
        //
        // // Read the Keycloak options to ensure required properties are available.
        // KeycloakOptions? keycloakOptions = keycloakConfig.Get<KeycloakOptions>();
        // ArgumentNullException.ThrowIfNull(keycloakOptions, nameof(keycloakOptions));
        //
        // // Add authentication with JWT Bearer using Keycloak settings.
        // services.AddAuthentication(options =>
        // {
        //     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        // })
        // .AddJwtBearer(options =>
        // {
        //     options.Authority = keycloakOptions.Authority;
        //     options.RequireHttpsMetadata = keycloakOptions.RequireHttpsMetadata;
        //     options.Audience = keycloakOptions.Audience;
        //
        //     // Optional: Add additional JWT Bearer configuration like token validation parameters here.
        //     // options.TokenValidationParameters = new TokenValidationParameters
        //     // {
        //     //     // Custom token validation configuration
        //     // };
        // });
        //
        // // Register the identity service.
        // services.AddScoped<IIdentityService, IdentityService>();
    }
}