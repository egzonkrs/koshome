using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Application.Users.Register;
using KosHome.Domain.Entities.Users;
using KosHome.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthenticationOptions _authOptions;
    private readonly IMediator _mediator;

    public AuthController(
        IOptions<AuthenticationOptions> authOptions,
        IMediator mediator)
    {
        _authOptions = authOptions.Value;
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        // Create identity user object
        var identityUser = new IdentityUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password,
            IsEnabled = true,
            IsEmailVerified = false
        };

        // Create and send the command
        var command = new RegisterUserCommand
        {
            IdentityUser = identityUser,
            RoleName = "user",
            RealmId = _authOptions.Keycloak.Realm
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailed)
        {
            return BadRequest(new { Errors = result.Errors });
        }

        return Ok(new { UserId = result.Value });
    }

    public class RegisterRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
} 