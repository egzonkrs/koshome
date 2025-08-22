using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using KosHome.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using KosHome.Api.Extensions.Controller;
using Microsoft.AspNetCore.Http;
using KosHome.Application.Countries.GetCountries;
using KosHome.Application.Countries.Create;
using KosHome.Application.Countries.Update;
using KosHome.Application.Countries.Delete;

namespace KosHome.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/countries")]
[ApiVersion("1")]
[Authorize]
public class CountryController : ControllerBase
{
    private readonly ISender _sender;

    public CountryController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Gets all countries.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IActionResult"/> containing the list of countries or an error.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CountryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCountries(CancellationToken cancellationToken = default)
    {
        var query = new GetAllCountriesQuery();
        var result = await _sender.Send(query, cancellationToken);
        return this.ToActionResult(result);
    }

    /// <summary>
    /// Gets a country by its unique identifier.
    /// </summary>
    /// <param name="countryId">The country identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IActionResult"/> containing the country or an error.</returns>
    [HttpGet("{countryId}")]
    [ProducesResponseType(typeof(CountryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountryById([FromRoute] Ulid countryId, CancellationToken cancellationToken = default)
    {
        var query = new GetCountryById(countryId);
        var result = await _sender.Send(query, cancellationToken);
        return this.ToActionResult(result);
    }
    
    /// <summary>
    /// Creates a new country.
    /// </summary>
    /// <param name="command">The create country command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IActionResult"/> containing the ID of the created country or an error.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Ulid), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateCountry([FromBody] CreateCountryCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(command, cancellationToken);
        return this.ToActionResult(result);
    }
    
    /// <summary>
    /// Updates an existing country.
    /// </summary>
    /// <param name="countryId">The ID of the country to update.</param>
    /// <param name="command">The update country command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IActionResult"/> indicating success or an error.</returns>
    [HttpPut("{countryId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateCountry([FromRoute] Ulid countryId, [FromBody] UpdateCountryCommand command, CancellationToken cancellationToken = default)
    {
        command.Id = countryId;
        var result = await _sender.Send(command, cancellationToken);
        return this.ToActionResult(result);
    }
    
    /// <summary>
    /// Deletes a country.
    /// </summary>
    /// <param name="countryId">The ID of the country to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An <see cref="IActionResult"/> indicating success or an error.</returns>
    [HttpDelete("{countryId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteCountry([FromRoute] Ulid countryId, CancellationToken cancellationToken = default)
    {
        var command = new DeleteCountryCommand(countryId);
        var result = await _sender.Send(command, cancellationToken);
        return this.ToActionResult(result);
    }
} 