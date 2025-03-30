using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KosHome.Application.Cities.GetCities;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RandomController : ControllerBase
{
    private readonly IMediator _mediator;

    public RandomController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = "GetRandomInt")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetCityById { CityId = Ulid.Parse("01JCVMN4DNJM1S45MGY6PKVVD3") }, cancellationToken);
        return Ok(result);
    }
}