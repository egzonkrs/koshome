using System;
using System.Security.Claims;
using System.Text.Json;
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

        options.Events ??= new JwtBearerEvents();

        if (_authOptions.Cookies.UseCookies)
        {
            options.Events.OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue(_authOptions.Cookies.Name, out var token))
                {
                    context.Token = token;
                }

                return System.Threading.Tasks.Task.CompletedTask;
            };
        }

        options.Events.OnTokenValidated = context =>
        {
            if (context.Principal?.Identity is ClaimsIdentity identity)
            {
                var realmAccess = context.Principal.FindFirst("realm_access")?.Value;
                if (!string.IsNullOrEmpty(realmAccess))
                {
                    using var document = JsonDocument.Parse(realmAccess);
                    if (document.RootElement.TryGetProperty("roles", out var realmRoles))
                    {
                        foreach (var role in realmRoles.EnumerateArray())
                        {
                            var roleName = role.GetString();
                            if (!string.IsNullOrWhiteSpace(roleName))
                            {
                                identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
                            }
                        }
                    }
                }

                var resourceAccess = context.Principal.FindFirst("resource_access")?.Value;
                if (!string.IsNullOrEmpty(resourceAccess))
                {
                    using var document = JsonDocument.Parse(resourceAccess);
                    foreach (var client in document.RootElement.EnumerateObject())
                    {
                        if (client.Value.TryGetProperty("roles", out var clientRoles))
                        {
                            foreach (var role in clientRoles.EnumerateArray())
                            {
                                var roleName = role.GetString();
                                if (!string.IsNullOrWhiteSpace(roleName))
                                {
                                    identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
                                }
                            }
                        }
                    }
                }
            }

            return System.Threading.Tasks.Task.CompletedTask;
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = !string.IsNullOrEmpty(_authOptions.Keycloak.Issuer),
            ValidIssuer = _authOptions.Keycloak.Issuer,
            ValidateAudience = !string.IsNullOrEmpty(_authOptions.Keycloak.Audience),
            ValidAudience = _authOptions.Keycloak.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            RoleClaimType = ClaimTypes.Role,
        };
    }
}
