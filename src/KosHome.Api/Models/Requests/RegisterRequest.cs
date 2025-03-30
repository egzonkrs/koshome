namespace KosHome.Api.Models.Requests;

public record RegisterRequest(string FirstName, string LastName, string Email, string Password);