using FluentResults;

namespace KosHome.Domain.Common;

/// <summary>
/// Provides error definitions for country-related operations.
/// </summary>
public static class CountriesErrors
{
    /// <summary>
    /// Returns an error indicating that an unexpected error occurred during a country operation.
    /// </summary>
    public static CustomFluentError UnexpectedError()
        => new CustomFluentError("COUNTRY_UNEXPECTED_ERROR", "An unexpected error occurred during the country operation.");

    /// <summary>
    /// Returns an error indicating that a country with the specified ID was not found.
    /// </summary>
    /// <param name="id">The ID of the country that was not found.</param>
    public static CustomFluentError NotFound(string id)
        => new CustomFluentError("COUNTRY_NOT_FOUND", $"Country with Id: `{id}` was not found.");

    /// <summary>
    /// Returns an error indicating that a country with the specified name already exists.
    /// </summary>
    /// <param name="countryName">The name of the country that already exists.</param>
    public static CustomFluentError AlreadyExists(string countryName)
        => new CustomFluentError("COUNTRY_ALREADY_EXISTS", $"A country with the name '{countryName}' already exists.");

    /// <summary>
    /// Returns an error indicating that the provided country name is invalid.
    /// </summary>
    /// <param name="countryName">The invalid country name.</param>
    public static CustomFluentError InvalidCountryName(string countryName)
        => new CustomFluentError("INVALID_COUNTRY_NAME", $"The country name '{countryName}' is invalid.");

    /// <summary>
    /// Returns an error indicating that the provided alpha3 code is invalid.
    /// </summary>
    /// <param name="alpha3Code">The invalid alpha3 code.</param>
    public static CustomFluentError InvalidAlpha3Code(string alpha3Code)
        => new CustomFluentError("INVALID_ALPHA3_CODE", $"The alpha3 code '{alpha3Code}' is invalid.");
} 