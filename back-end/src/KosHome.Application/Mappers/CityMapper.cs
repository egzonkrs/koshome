using KosHome.Application.Cities.GetCities;
using KosHome.Domain.Entities.Cities;

namespace KosHome.Application.Mappers;

/// <summary>
/// Provides mapping methods for City entities.
/// </summary>
public static class CityMappingProfile
{
    /// <summary>
    /// Maps a City entity to a CityResponse.
    /// </summary>
    public static CityResponse ToResponse(this City city)
    {
        return new CityResponse(
            city.Id,
            city.CityName.Value,
            city.Alpha3Code.Value,
            city.CountryId
        );
    }
}