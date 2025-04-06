using FluentResults;

namespace KosHome.Domain.Common;

/// <summary>
/// Provides error definitions for city-related operations.
/// </summary>
public static class CitiesErrors
{
    /// <summary>
    /// Returns an error indicating that an unexpected error occurred during a city operation.
    /// </summary>
    public static Error UnexpectedError()
        => new CustomFluentError("CITY_UNEXPECTED_ERROR", "An unexpected error occurred during the city operation.");

    /// <summary>
    /// Returns an error indicating that a city with the specified ID was not found.
    /// </summary>
    /// <param name="id">The ID of the city that was not found.</param>
    public static Error NotFound(string id)
        => new CustomFluentError("CITY_NOT_FOUND", $"City with Id: `{id}` was not found.");

    /// <summary>
    /// Returns an error indicating that a city with the specified name already exists.
    /// </summary>
    /// <param name="cityName">The name of the city that already exists.</param>
    public static Error AlreadyExists(string cityName)
        => new CustomFluentError("CITY_ALREADY_EXISTS", $"A city with the name '{cityName}' already exists.");

    /// <summary>
    /// Returns an error indicating that the provided city name is invalid.
    /// </summary>
    /// <param name="cityName">The invalid city name.</param>
    public static Error InvalidCityName(string cityName)
        => new CustomFluentError("INVALID_CITY_NAME", $"The city name '{cityName}' is invalid.");

    /// <summary>
    /// Returns an error indicating that the provided alpha3 code is invalid.
    /// </summary>
    /// <param name="alpha3Code">The invalid alpha3 code.</param>
    public static Error InvalidAlpha3Code(string alpha3Code)
        => new CustomFluentError("INVALID_ALPHA3_CODE", $"The alpha3 code '{alpha3Code}' is invalid.");

    /// <summary>
    /// Returns an error indicating that no country was found for the given city.
    /// </summary>
    /// <param name="countryId">The ID of the country.</param>
    public static Error CountryNotFound(string countryId)
        => new CustomFluentError("COUNTRY_NOT_FOUND_FOR_CITY", $"No country found with Id: `{countryId}` for this city.");
}