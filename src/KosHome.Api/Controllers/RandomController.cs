// using System;
// using System.Threading;
// using System.Threading.Tasks;
// using KosHome.Application.Abstractions.Auth.Services;
// using KosHome.Domain.Entities.Users;
// using KosHome.Infrastructure.Authentication;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Options;
// using KosHome.Application.Cities.GetCities;
// using MediatR;
//
// namespace KosHome.Api.Controllers;
//
// [ApiController]
// [Route("api/[controller]")]
// public class RandomController : ControllerBase
// {
//     private readonly IKeycloakIdentityService _keycloakIdentityService;
//     private readonly AuthenticationOptions _authOptions;
//     private readonly IMediator _mediator;
//
//     public RandomController(
//         IKeycloakIdentityService keycloakIdentityService,
//         IOptions<AuthenticationOptions> authOptions,
//         IMediator mediator)
//     {
//         _keycloakIdentityService = keycloakIdentityService;
//         _authOptions = authOptions.Value;
//         _mediator = mediator;
//     }
//
//     [HttpPost("register")]
//     public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
//     {
//         var identityUser = new IdentityUser
//         {
//             FirstName = request.FirstName,
//             LastName = request.LastName,
//             Email = request.Email,
//             Password = request.Password,
//             IsEnabled = true,
//             IsEmailVerified = false
//         };
//
//         var result = await _keycloakIdentityService.RegisterIdentityUserAsync(identityUser, cancellationToken);
//
//         if (result.IsSuccess)
//         {
//             return Ok(new { UserId = result.Value });
//         }
//
//         return BadRequest(new { Errors = result.Errors });
//     }
//
//     public class RegisterRequest
//     {
//         public string FirstName { get; set; } = string.Empty;
//         public string LastName { get; set; } = string.Empty;
//         public string Email { get; set; } = string.Empty;
//         public string Password { get; set; } = string.Empty;
//     }
//
//     [HttpGet(Name = "GetRandomInt")]
//     public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
//     {
//         var cityId = Ulid.Parse("01JCVMN4DNJM1S45MGY6PKVVD0");
//         var result = await _mediator.Send(new GetCityById
//         {
//             CityId = cityId
//         }, cancellationToken);
//
//         return Ok(result);
//     }
// }