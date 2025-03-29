using System;
using Keycloak.Net;
using KosHome.Application.Abstractions.Auth.Services;
using KosHome.Domain.Abstractions;
using KosHome.Infrastructure.Authentication;
using KosHome.Infrastructure.Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using KeycloakOptions = KosHome.Infrastructure.Authentication.KeycloakOptions;

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
        services.Configure<AuthenticationOptions>(authSection);
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();

        services.AddSingleton<Keycloak.Net.KeycloakClient>(provider =>
        {
            var keycloakOptions = keycloakSection.Get<KeycloakOptions>();
            return new KeycloakClient(
                keycloakOptions.Authority,      // e.g., http://localhost:9090
                keycloakOptions.ClientSecret,   // Your actual client secret from appsettings
                new Keycloak.Net.KeycloakOptions(
                    authenticationRealm: keycloakOptions.Realm, // Realm for authentication (koshome)
                    adminClientId: keycloakOptions.ClientId    // The Client ID itself (koshome-client)
                    // Prefix is "" by default, which is usually fine
                )
            );
        });
        services.AddScoped<IKeycloakIdentityService, KeycloakClientWrapper>();
        services.AddScoped<IIdentityService, KeycloakIdentityService>();
    }
}