using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using KosHome.Api.Extensions;
using KosHome.Api.Models;
using KosHome.Api.Models.Common;
using KosHome.Domain.Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using KosHome.Application.Cities.GetCities;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using KosHome.Api.Extensions.Controller;
using KosHome.Api.Extensions.Common;
using KosHome.Application.Cities.Create;
using Microsoft.AspNetCore.Http;
using KosHome.Application.Cities.Update;
using KosHome.Application.Cities.Delete;
using KosHome.Domain.Common;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/cities")]
[ApiVersion("1")]
[Authorize]
public class CityController : ControllerBase
{
    private readonly ISender _sender;

    public CityController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Gets cities with pagination and filtering support.
    /// </summary>
    /// <param name="request">The filter and pagination request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paginated list of cities.</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<CityResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCities([FromQuery] Models.Cities.CityFilterRequest request, CancellationToken cancellationToken = default)
    {
        var query = new GetAllCitiesQuery
        {
            PaginationRequest = request.ToDomain(),
            CountryId = request.CountryId
        };

        var result = await _sender.Send(query, cancellationToken);
        return this.ToPaginatedActionResult(result);
    }

    /// <summary>
    /// Gets a city by its unique identifier.
    /// </summary>
    /// <param name="cityId">The city identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IActionResult"/> containing the city or an error.</returns>
    [HttpGet("{cityId}")]
    [ProducesResponseType(typeof(CityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCityById([FromRoute] Ulid cityId, CancellationToken cancellationToken = default)
    {
        var query = new GetCityById(cityId);
        var result = await _sender.Send(query, cancellationToken);
        return this.ToActionResult(result);
    }
    
    /// <summary>
    /// Creates a new city.
    /// </summary>
    /// <param name="command">The create city command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IActionResult"/> containing the ID of the created city or an error.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Ulid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> CreateCity([FromBody] CreateCityCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(command, cancellationToken);
        return this.ToActionResult(result);
    }
    
    /// <summary>
    /// Updates an existing city.
    /// </summary>
    /// <param name="cityId">The ID of the city to update.</param>
    /// <param name="command">The update city command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IActionResult"/> indicating success or an error.</returns>
    [HttpPut("{cityId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> UpdateCity([FromRoute] Ulid cityId, [FromBody] UpdateCityCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = cityId;
        var result = await _sender.Send(command, cancellationToken);
        return this.ToActionResult(result);
    }
    
    /// <summary>
    /// Deletes a city.
    /// </summary>
    /// <param name="cityId">The ID of the city to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IActionResult"/> indicating success or an error.</returns>
    [HttpDelete("{cityId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> DeleteCity([FromRoute] Ulid cityId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteCityCommand(cityId);
        var result = await _sender.Send(command, cancellationToken);
        return this.ToActionResult(result);
    }
}