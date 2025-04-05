using System;

namespace KosHome.Application.Cities.GetCities;

public sealed class CityResponse
{
    public Ulid Id { get; set; }
    public string CityName { get; set; }
    public string CityAlpha3Code { get; set; }
    public Ulid CountryId { get; set; }
}