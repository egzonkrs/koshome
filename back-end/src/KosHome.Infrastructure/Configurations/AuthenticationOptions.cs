using KosHome.Infrastructure.Authentication;

namespace KosHome.Infrastructure.Configurations;

public sealed class AuthenticationOptions
{
    public const string SectionName = "Authentication";
    public KeycloakOptions Keycloak { get; init; } = new();
    public CookiesSettings Cookies { get; init; } = new();

    public sealed class CookiesSettings
    {
        public string Name { get; init; } = "koshome_auth";
        public bool UseCookies { get; init; } = true;
        public bool HttpOnly { get; init; } = true;
        public bool SecureOnly { get; init; } = true;
        public string SameSite { get; init; } = "Strict";
        public int ExpirationMinutes { get; init; } = 60;
    }
}