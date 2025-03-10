namespace KosHome.Infrastructure.Authentication;

public sealed class AuthenticationOptions
{
    public const string SectionName = "Authentication";
    
    public string Audience { get; init; } = string.Empty;
    public string MetadataUrl { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public bool RequireHttpsMetadata { get; init; } = true;
    
    // Keycloak specific settings
    public string Authority { get; init; } = string.Empty;
    public string Realm { get; init; } = "koshome";
    public string AdminUsername { get; init; } = string.Empty;
    public string AdminPassword { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
    
    // Cookie settings
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