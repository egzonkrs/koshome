using System;
using KosHome.Domain.Abstractions;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Events.Cities;
using KosHome.Domain.ValueObjects.Cities;
using KosHome.Domain.ValueObjects.Countries;

namespace KosHome.Domain.Entities.Cities;
/// <summary>
/// Represents a city entity.
/// </summary>
public sealed class City : DomainEntity, IEntity
{
    private City(
        Ulid id,
        CityName cityName,
        CityAlpha3Code cityAlpha3Code,
        Ulid countryId)
    {
        Id = id;
        CityName = cityName;
        CountryId = countryId;
        Alpha3Code = cityAlpha3Code;
    }

    private City()
    {
    }
    
    /// <summary>
    /// The Id of the city.
    /// </summary>
    public Ulid Id { get; set; }

    /// <summary>
    /// Gets the city name.
    /// </summary>
    public CityName CityName { get; private set; }

    /// <summary>
    /// Gets the city Alpha3Code.
    /// </summary>
    public CityAlpha3Code Alpha3Code { get; private set; }
    
    /// <summary>
    /// Gets the country identifier.
    /// </summary>
    public Ulid CountryId { get; private set; }

    /// <summary>
    /// Creates a new city instance.
    /// </summary>
    /// <param name="cityName">Name of the city.</param>
    /// <param name="cityAlpha3Code">The Alpha3Code of the city.</param>
    /// <param name="countryId">Identifier of the country.</param>
    /// <returns>A new <see cref="City"/> instance.</returns>
    public static City Create(
        CityName cityName,
        CityAlpha3Code cityAlpha3Code,
        Ulid countryId)
    {
        var city = new City(Ulid.NewUlid(), cityName, cityAlpha3Code, countryId);

        city.RaiseDomainEvent(new CityCreatedDomainEvent(city.Id));
        return city;
    }
}
