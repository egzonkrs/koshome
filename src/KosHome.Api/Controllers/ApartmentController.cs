using System;
using MediatR;
using Asp.Versioning;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Api.Extensions;
using KosHome.Api.Extensions.Common;
using KosHome.Api.Extensions.Controller;
using KosHome.Api.Models;
using KosHome.Api.Models.Apartments.Requests;
using KosHome.Api.Models.Common;
using KosHome.Application.Apartments.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KosHome.Application.Apartments.CreateApartment;
using KosHome.Application.Apartments.GetApartments;
using KosHome.Domain.Common;
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

    /// <summary>
    /// Gets apartments with pagination and filtering support.
    /// </summary>
    /// <param name="request">The filter and pagination request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paginated list of apartments.</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<ApartmentResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApartments([FromQuery] Models.Apartments.ApartmentFilterRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetApartmentsQuery
        {
            PaginationRequest = request.ToDomain(),
            CityId = request.CityId,
            MinPrice = request.MinPrice,
            MaxPrice = request.MaxPrice
        };

        var result = await _mediator.Send(query, cancellationToken);
        return this.ToPaginatedActionResult(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Ulid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Create([FromForm] CreateApartmentRequest request, CancellationToken cancellationToken)
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
            Longitude = request.Longitude,
            Images = request.Images
        }, cancellationToken);

        return this.ToActionResult(result);
    }
} 