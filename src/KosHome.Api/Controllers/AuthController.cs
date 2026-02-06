using MediatR;
using Asp.Versioning;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Api.Extensions;
using KosHome.Api.Extensions.Controller;
using KosHome.Api.Filters;
using KosHome.Api.Models;
using KosHome.Api.Models.Authentication.Requests;
using KosHome.Application.Abstractions.Auth.Constants;
using KosHome.Application.Users.GetCurrentUser;
using KosHome.Domain.Abstractions;
using KosHome.Domain.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KosHome.Application.Users.Login;
using KosHome.Application.Users.Register;
using KosHome.Domain.Entities.Users;
using RegisterRequest = KosHome.Api.Models.Authentication.Requests.RegisterRequest;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserContextAccessor _userContext;
    private readonly IUserRepository _userRepository;

    public AuthController(IMediator mediator, IUserContextAccessor userContext, IUserRepository userRepository)
    {
        _mediator = mediator;
        _userContext = userContext;
        _userRepository = userRepository;
    }

    [HttpPost("signup")]
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
    [ServiceFilter(typeof(JwtCookieResultFilter))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var loginResult = await _mediator.Send(new LoginUserCommand(request.Email, request.Password), cancellationToken);
        return this.ToActionResult(loginResult);
    }
} 