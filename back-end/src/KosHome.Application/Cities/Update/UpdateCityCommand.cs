using System;
using FluentResults;
using MediatR;

namespace KosHome.Application.Cities.Update;

/// <summary>
/// Command to update an existing city.
/// </summary>
public sealed class UpdateCityCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// The ID of the city to update.
    /// </summary>
    public Ulid Id { get; set; }
    
    /// <summary>
    /// The updated name of the city.
    /// </summary>
    public string CityName { get; set; }

    /// <summary>
    /// The updated alpha-3 code of the city.
    /// </summary>
    public string Alpha3Code { get; set; }

    /// <summary>
    /// The updated ID of the country this city belongs to.
    /// </summary>
    public Ulid CountryId { get; set; }
} 