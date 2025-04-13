using MediatR;
using Asp.Versioning;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Api.Extensions;
using KosHome.Application.Abstractions.Auth.Constants;
using Microsoft.AspNetCore.Mvc;
using KosHome.Application.Users.Login;
using Microsoft.AspNetCore.Identity.Data;
using KosHome.Application.Users.Register;
using KosHome.Domain.Entities.Users;
using RegisterRequest = KosHome.Api.Models.Requests.RegisterRequest;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("v{version:apiVersion}/auth")]
[ApiVersion("1")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var registerResult = await _mediator.Send(new RegisterUserCommand
        {
            RoleName = Constants.PlayerRole,
            IdentityUser = new IdentityUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                IsEnabled = true,
                IsEmailVerified = false
            }
        }, cancellationToken);

        return this.ToActionResult(registerResult);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var loginResult = await _mediator.Send(new LoginUserCommand(request.Email, request.Password), cancellationToken);
        return this.ToActionResult(loginResult);
    }
} 