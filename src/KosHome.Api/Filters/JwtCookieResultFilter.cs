using System;
using System.Threading.Tasks;
using KosHome.Application.Users.Login;
using KosHome.Infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace KosHome.Api.Filters;

/// <summary>
/// A result filter that sets an HTTP‑only cookie for the access token if the action returns a LoginResponse.
/// </summary>
public class JwtCookieResultFilter : IAsyncResultFilter
{
    private readonly AuthenticationOptions _authOptions;

    public JwtCookieResultFilter(IOptions<AuthenticationOptions> authOptions)
    {
        _authOptions = authOptions.Value;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is OkObjectResult okResult && okResult.Value is LoginResponse response)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // true in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(5) // Should match your token lifetime
            };

            context.HttpContext.Response.Cookies.Append(_authOptions.Cookie.Name, response.AccessToken, cookieOptions);
        }

        await next();
    }
}