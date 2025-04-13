namespace KosHome.Application.Users.Login;

public record LoginResponse
{
    public string AccessToken { get; init; }
}