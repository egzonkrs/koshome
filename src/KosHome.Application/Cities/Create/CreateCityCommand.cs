using System;
using FluentResults;
using KosHome.Domain.ValueObjects.Cities;
using MediatR;

namespace KosHome.Application.Cities.Create;

/// <summary>
/// Command to create a new city.
/// </summary>
public sealed class CreateCityCommand : IRequest<Result<Ulid>>
{
    /// <summary>
    /// The name of the city.
    /// </summary>
    public string CityName { get; set; }

    /// <summary>
    /// The alpha-3 code of the city.
    /// </summary>
    public string Alpha3Code { get; set; }

    /// <summary>
    /// The ID of the country this city belongs to.
    /// </summary>
    public Ulid CountryId { get; set; }
}
