using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KosHome.Infrastructure.Authentication;

public sealed class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly AuthenticationOptions _authOptions;

    public JwtBearerOptionsSetup(IOptions<AuthenticationOptions> authOptions)
    {
        _authOptions = authOptions.Value;
    }

    public void Configure(JwtBearerOptions options)
    {
        Configure(JwtBearerDefaults.AuthenticationScheme, options);
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        if (name != JwtBearerDefaults.AuthenticationScheme)
        {
            return;
        }

        options.Authority = _authOptions.Authority;
        options.Audience = _authOptions.Audience;
        options.RequireHttpsMetadata = _authOptions.RequireHttpsMetadata;
        options.MetadataAddress = _authOptions.MetadataUrl;
        
        if (_authOptions.UseCookies)
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // Extract the token from the cookie
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
            ValidateIssuer = !string.IsNullOrEmpty(_authOptions.Issuer),
            ValidIssuer = _authOptions.Issuer,
            ValidateAudience = !string.IsNullOrEmpty(_authOptions.Audience),
            ValidAudience = _authOptions.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    }
}