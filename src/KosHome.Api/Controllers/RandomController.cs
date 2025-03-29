using System;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Application.Abstractions.Auth.Services;
using KosHome.Domain.Entities.Users;
using KosHome.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using KosHome.Application.Cities.GetCities;
using MediatR;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RandomController : ControllerBase
{
    private readonly IMediator _mediator;

    public RandomController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = "GetRandomInt")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var cityId = Ulid.Parse("01JCVMN4DNJM1S45MGY6PKVVD4");
        var result = await _mediator.Send(new GetCityById
        {
            CityId = cityId
        }, cancellationToken);

        return Ok(result);
    }
}