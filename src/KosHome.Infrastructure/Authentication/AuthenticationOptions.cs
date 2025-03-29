namespace KosHome.Infrastructure.Authentication;

public sealed class AuthenticationOptions
{
    public const string SectionName = "Authentication";
    
    // Nest Keycloak settings under "Keycloak"
    public KeycloakOptions Keycloak { get; init; } = new();

    // Other authentication settings (e.g., cookie settings)
    public bool UseCookies { get; init; } = true;
    public CookieSettings Cookie { get; init; } = new();

    public sealed class CookieSettings
    {
        public string Name { get; init; } = "koshome_auth";
        public bool HttpOnly { get; init; } = true;
        public bool SecureOnly { get; init; } = true;
        public string SameSite { get; init; } = "Strict";
        public int ExpirationMinutes { get; init; } = 60;
    }
}