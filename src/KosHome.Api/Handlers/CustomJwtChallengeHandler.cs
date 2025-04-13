using System.Collections.Generic;
using System.Threading.Tasks;
using KosHome.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace KosHome.Api.Handlers;

public static class CustomJwtChallengeHandler
{
    public static async Task HandleAsync(JwtBearerChallengeContext context)
    {
        context.HandleResponse();
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new ApiResponse<object>
        {
            IsFailed = true,
            Errors = new Dictionary<string, string>()
            {
                { "UNAUTHORIZED_ACCESS", "You are not authorized to access this resource" }
            }
        });
    }
}