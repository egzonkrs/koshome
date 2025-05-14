using System;
using MediatR;
using Asp.Versioning;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Api.Extensions;
using KosHome.Api.Extensions.Controller;
using KosHome.Api.Models.Apartments.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KosHome.Application.Apartments.CreateApartment;
using Microsoft.AspNetCore.Http;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/apartments")]
[ApiVersion("1")]
[Authorize]
public class ApartmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApartmentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Ulid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateApartmentRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateApartmentCommand
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            ListingType = request.ListingType,
            PropertyType = request.PropertyType,
            Address = request.Address,
            CityId = request.CityId,
            Bedrooms = request.Bedrooms,
            Bathrooms = request.Bathrooms,
            SquareMeters = request.SquareMeters,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        }, cancellationToken);

        return this.ToActionResult(result);
    }
} 