using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KosHome.Application.Cities.GetCities;
using Microsoft.AspNetCore.Authorization;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RandomController : ControllerBase
{
    private readonly IMediator _mediator;

    public RandomController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = "GetRandomInt")]
    public async Task<IActionResult> Get([FromQuery]Ulid cityId, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetCityById { CityId = cityId }, cancellationToken);
        var qwe = result.ValueOrDefault;

        try
        {
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Ok(result.ToResult());
        }
        
        return Ok(result.ToResult());
    }
}