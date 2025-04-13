using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using KosHome.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using KosHome.Application.Cities.GetCities;
using Microsoft.AspNetCore.Authorization;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("v{version:apiVersion}/city")]
[ApiVersion("1")]
[Authorize]
public class CityController : ControllerBase
{
    private readonly IMediator _mediator;

    public CityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery]Ulid cityId, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetCityById { CityId = cityId }, cancellationToken);
        return this.ToActionResult(result);
    }
}