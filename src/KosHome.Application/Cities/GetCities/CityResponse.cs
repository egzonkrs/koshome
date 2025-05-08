using System;

namespace KosHome.Application.Cities.GetCities;

/// <summary>
/// Represents the data transfer object for a city.
/// </summary>
public sealed record CityResponse(
    Ulid Id,
    string CityName,
    string CityAlpha3Code,
    Ulid CountryId
);