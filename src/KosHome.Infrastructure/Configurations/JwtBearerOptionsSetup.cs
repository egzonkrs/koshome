using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KosHome.Infrastructure.Configurations;

public sealed class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly AuthenticationOptions _authOptions;

    public JwtBearerOptionsSetup(IOptions<AuthenticationOptions> authOptions)
    {
        _authOptions = authOptions.Value ?? throw new ArgumentNullException(nameof(authOptions), "Authentication options cannot be null");
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        if (name is null or not JwtBearerDefaults.AuthenticationScheme)
        {
            return;
        }

        options.Authority = _authOptions.Keycloak.Authority;
        options.Audience = _authOptions.Keycloak.Audience;
        options.MetadataAddress = _authOptions.Keycloak.MetadataUrl;
        options.RequireHttpsMetadata = _authOptions.Keycloak.RequireHttpsMetadata;
        
        if (_authOptions.UseCookies)
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (context.Request.Cookies.TryGetValue(_authOptions.Cookie.Name, out var token))
                    {
                        context.Token = token;
                    }
                    return System.Threading.Tasks.Task.CompletedTask;
                }
            };
        }
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = !string.IsNullOrEmpty(_authOptions.Keycloak.Issuer),
            ValidIssuer = _authOptions.Keycloak.Issuer,
            ValidateAudience = !string.IsNullOrEmpty(_authOptions.Keycloak.Audience),
            ValidAudience = _authOptions.Keycloak.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }
}
