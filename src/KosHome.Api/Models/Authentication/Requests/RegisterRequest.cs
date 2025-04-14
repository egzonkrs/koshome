namespace KosHome.Api.Models.Authentication.Requests;

public record RegisterRequest(string FirstName, string LastName, string Email, string Password);