namespace KosHome.Infrastructure.Authentication.Models;

public sealed class TokenResponse
{
    public string AccessToken { get; init; }
    public int ExpiresIn { get; init; }
    public int RefreshExpiresIn { get; init; }
    public string RefreshToken { get; init; }
    public string TokenType { get; init; }
    public string NotBeforePolicy { get; init; }
    public string SessionState { get; init; }
    public string Scope { get; init; }
}