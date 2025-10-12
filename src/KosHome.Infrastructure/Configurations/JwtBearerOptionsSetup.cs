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
        var isJwtScheme = name is JwtBearerDefaults.AuthenticationScheme;
        if (isJwtScheme is false)
        {
            return;
        }

        options.Authority = _authOptions.Keycloak.Authority;
        options.Audience = _authOptions.Keycloak.Audience;
        options.MetadataAddress = _authOptions.Keycloak.MetadataUrl;
        options.RequireHttpsMetadata = _authOptions.Keycloak.RequireHttpsMetadata;

        options.Events ??= new JwtBearerEvents();

        var shouldUseCookies = _authOptions.Cookies.UseCookies;
        if (shouldUseCookies)
        {
            options.Events.OnMessageReceived = context =>
            {
                var hasTokenCookie = context.Request.Cookies.TryGetValue(_authOptions.Cookies.Name, out var token);
                if (hasTokenCookie)
                {
                    context.Token = token;
                }

                return System.Threading.Tasks.Task.CompletedTask;
            };
        }

        options.Events.OnTokenValidated = context =>
        {
            if (context.Principal?.Identity is not ClaimsIdentity identity)
            {
                return System.Threading.Tasks.Task.CompletedTask;
            }

            var realmAccess = context.Principal.FindFirst("realm_access")?.Value;
            var hasRealmAccess = !string.IsNullOrEmpty(realmAccess);
            if (hasRealmAccess)
            {
                using var realmDocument = JsonDocument.Parse(realmAccess);
                var hasRealmRoles = realmDocument.RootElement.TryGetProperty("roles", out var realmRoles);
                if (hasRealmRoles)
                {
                    foreach (var role in realmRoles.EnumerateArray())
                    {
                        var roleName = role.GetString();
                        var hasRoleName = !string.IsNullOrWhiteSpace(roleName);
                        if (hasRoleName)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
                        }
                    }
                }
            }

            var resourceAccess = context.Principal.FindFirst("resource_access")?.Value;
            var hasResourceAccess = !string.IsNullOrEmpty(resourceAccess);
            if (hasResourceAccess is false)
            {
                return System.Threading.Tasks.Task.CompletedTask;
            }

            using var resourceDocument = JsonDocument.Parse(resourceAccess);
            foreach (var client in resourceDocument.RootElement.EnumerateObject())
            {
                var hasClientRoles = client.Value.TryGetProperty("roles", out var clientRoles);
                if (hasClientRoles is false)
                {
                    continue;
                }

                foreach (var role in clientRoles.EnumerateArray())
                {
                    var roleName = role.GetString();
                    var hasRoleName = !string.IsNullOrWhiteSpace(roleName);
                    if (hasRoleName is false)
                    {
                        continue;
                    }

                    identity.AddClaim(new Claim(ClaimTypes.Role, roleName));
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
