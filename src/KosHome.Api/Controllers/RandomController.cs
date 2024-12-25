using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Application.Cities.GetCities;
using KosHome.Domain.Data.Repositories;
using MediatR;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("[controller]")]
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
        var cityId = Ulid.Parse("01JCVMN4DNJM1S45MGY6PKVVD0");
        var result = await _mediator.Send(new GetCityById
        {
            CityId = cityId
        }, cancellationToken);

        return Ok(result);
    }
}