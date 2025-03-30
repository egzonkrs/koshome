using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Application.Abstractions.Auth.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KosHome.Application.Users.Login;

/// <summary>
/// Handles the login command by authenticating the user through Keycloak. Returns the access token on success.
/// </summary>
public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<string>>
{
    private readonly IKeycloakIdentityService _identityService;
    private readonly ILogger<LoginUserCommandHandler> _logger;

    public LoginUserCommandHandler(IKeycloakIdentityService identityService, ILogger<LoginUserCommandHandler> logger)
    {
        _identityService = identityService;
        _logger = logger;
    }

    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var loginResult = await _identityService.LoginAsync(request.Email, request.Password, cancellationToken);
        if (loginResult.IsFailed)
        {
            _logger.LogError("Login failed for {Email}", request.Email);
            return Result.Fail<string>("Invalid credentials or error during login.");
        }
        
        return Result.Ok(loginResult.Value);
    }
}