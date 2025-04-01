using System.Threading;
using System.Threading.Tasks;
using KosHome.Application.Users.Login;
using KosHome.Application.Users.Register;
using KosHome.Domain.Entities.Users;
using KosHome.Infrastructure.Authentication;
using KosHome.Infrastructure.Configurations;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RegisterRequest = KosHome.Api.Models.Requests.RegisterRequest;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("api/identity-users")]
public class IdentityUsersController : ControllerBase
{
    private readonly string _keycloakRealm;
    private readonly IMediator _mediator;

    public IdentityUsersController(
        IOptions<AuthenticationOptions> authOptions,
        IMediator mediator)
    {
        _mediator = mediator;
        _keycloakRealm = authOptions.Value.Keycloak.Realm;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var registerUserCommand = new RegisterUserCommand
        {
            RoleName = "user",
            RealmId = _keycloakRealm,
            IdentityUser = new IdentityUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                IsEnabled = true,
                IsEmailVerified = false
            }
        };

        var result = await _mediator.Send(registerUserCommand, cancellationToken);

        if (result.IsFailed)
        {
            return BadRequest(new { Errors = result.Errors });
        }

        return Ok(new { UserId = result.Value });
    }

    [HttpPost("login", Name = nameof(Login))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await _mediator.Send(command, cancellationToken);
        if (result.IsFailed)
        {
            return Unauthorized(new { Errors = result.Errors });
        }
        return Ok(result);
    }
} 