using System;

namespace KosHome.Application.Countries.GetCountries;

/// <summary>
/// Represents the data transfer object for a country.
/// </summary>
public sealed record CountryResponse(
    Ulid Id,
    string CountryName,
    string Alpha3Code
); 