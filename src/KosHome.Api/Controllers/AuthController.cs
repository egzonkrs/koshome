using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KosHome.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationOptions _authOptions;

    public AuthController(
        HttpClient httpClient,
        IOptions<AuthenticationOptions> authOptions)
    {
        _httpClient = httpClient;
        _authOptions = authOptions.Value;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            // Prepare the token request to Keycloak
            var tokenRequest = new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = _authOptions.ClientId,
                ["client_secret"] = _authOptions.ClientSecret,
                ["username"] = request.Email,
                ["password"] = request.Password
            };

            // Get the token from Keycloak
            var tokenEndpoint = $"{_authOptions.Authority}/realms/{_authOptions.Realm}/protocol/openid-connect/token";
            var response = await _httpClient.PostAsync(
                tokenEndpoint,
                new FormUrlEncodedContent(tokenRequest));

            if (!response.IsSuccessStatusCode)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
            
            if (tokenResponse == null)
            {
                return StatusCode(500, new { Message = "Failed to process authentication response" });
            }

            // Set the JWT token as a cookie
            if (_authOptions.UseCookies)
            {
                Response.Cookies.Append(
                    _authOptions.Cookie.Name,
                    tokenResponse.AccessToken,
                    new CookieOptions
                    {
                        HttpOnly = _authOptions.Cookie.HttpOnly,
                        Secure = _authOptions.Cookie.SecureOnly,
                        SameSite = _authOptions.Cookie.SameSite switch
                        {
                            "Strict" => SameSiteMode.Strict,
                            "Lax" => SameSiteMode.Lax,
                            "None" => SameSiteMode.None,
                            _ => SameSiteMode.Strict
                        },
                        Expires = DateTimeOffset.Now.AddMinutes(_authOptions.Cookie.ExpirationMinutes)
                    });
                
                // Don't return the token in the response body when using cookies
                return Ok(new { Message = "Login successful" });
            }

            // If not using cookies, return the token in the response
            return Ok(new { Token = tokenResponse.AccessToken });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = $"An error occurred: {ex.Message}" });
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        if (_authOptions.UseCookies)
        {
            Response.Cookies.Delete(_authOptions.Cookie.Name);
        }
        
        return Ok(new { Message = "Logged out successfully" });
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    private class TokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; } = string.Empty;
    }
} 