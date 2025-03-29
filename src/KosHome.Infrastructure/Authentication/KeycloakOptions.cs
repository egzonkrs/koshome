namespace KosHome.Infrastructure.Authentication;

public sealed class KeycloakOptions
{
    public const string SectionName = "Keycloak";
    
    public string Authority { get; init; } = string.Empty;
    public string Realm { get; init; } = "koshome";
    public string AdminUsername { get; init; } = string.Empty;
    public string AdminPassword { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string MetadataUrl { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public bool RequireHttpsMetadata { get; init; } = true;
}