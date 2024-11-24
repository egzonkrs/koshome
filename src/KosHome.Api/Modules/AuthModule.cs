using System;
using KosHome.Domain.Abstractions;
using KosHome.Infrastructure.Authentication;
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
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        var authenticationOptions = _configuration.GetSection(AuthenticationOptions.SectionName);
        ArgumentNullException.ThrowIfNull(authenticationOptions);

        services.Configure<AuthenticationOptions>(authenticationOptions);
    }
}