using MediatR;
using Asp.Versioning;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Api.Extensions;
using KosHome.Api.Models.Apartments.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KosHome.Application.Apartments.CreateApartment;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/apartment")]
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
            LocationId = request.LocationId,
            Bedrooms = request.Bedrooms,
            Bathrooms = request.Bathrooms,
            SquareMeters = request.SquareMeters,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        }, cancellationToken);

        return this.ToActionResult(result);
    }
} 