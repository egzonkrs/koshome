using System;
using System.Linq;
using MediatR;
using Asp.Versioning;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Api.Extensions;
using KosHome.Api.Extensions.Controller;
using KosHome.Api.Extensions.Common;
using KosHome.Api.Models;
using KosHome.Api.Models.Apartments.Requests;
using KosHome.Api.Models.Common;
using KosHome.Application.Apartments.Common;
using KosHome.Domain.Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KosHome.Application.Apartments.CreateApartment;
using KosHome.Application.Apartments.GetApartments;
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
    /// <param name="paginationRequest">The pagination parameters.</param>
    /// <param name="cityId">Optional city filter.</param>
    /// <param name="minPrice">Optional minimum price filter.</param>
    /// <param name="maxPrice">Optional maximum price filter.</param>
    /// <param name="searchTerm">Optional search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paginated list of apartments.</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<ApartmentResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApartments(
        [FromQuery] Models.Common.PaginationRequest paginationRequest,
        [FromQuery] Ulid? cityId = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetApartmentsQuery
        {
            PaginationRequest = paginationRequest.ToDomain(),
            CityId = cityId,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            SearchTerm = searchTerm
        };

        var result = await _mediator.Send(query, cancellationToken);
        return this.ToPaginatedActionResult(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Ulid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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