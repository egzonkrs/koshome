namespace KosHome.Infrastructure.Authentication;

public sealed class AuthenticationOptions
{
    public const string SectionName = "Authentication";
    
    public string Audience { get; init; } = string.Empty;
    public string MetadataUrl { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public bool RequireHttpsMetadata { get; init; }
}