using KosHome.Application.Countries.GetCountries;
using KosHome.Domain.Entities.Countries;

namespace KosHome.Application.Mappers;

/// <summary>
/// Provides mapping methods for Country entities.
/// </summary>
public static class CountryMappingProfile
{
    /// <summary>
    /// Maps a Country entity to a CountryResponse.
    /// </summary>
    public static CountryResponse ToResponse(this Country country)
    {
        return new CountryResponse(
            country.Id,
            country.CountryName.Value,
            country.Alpha3Code.Value
        );
    }
} 