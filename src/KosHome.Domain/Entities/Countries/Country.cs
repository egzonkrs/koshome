using System;
using KosHome.Domain.Abstractions;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Events.Countries;
using KosHome.Domain.ValueObjects.Countries;

namespace KosHome.Domain.Entities.Countries;

/// <summary>
/// Represents a country entity.
/// </summary>
public sealed class Country : DomainEntity
{
    private Country(
        Ulid id,
        CountryName countryName,
        CountryAlpha3Code alpha3Code)
    {
        Id = id;
        CountryName = countryName;
        Alpha3Code = alpha3Code;
    }

    private Country()
    {
    }

    /// <summary>
    /// The Id of the country.
    /// </summary>
    public Ulid Id { get; private set; }
    
    /// <summary>
    /// Gets the country name.
    /// </summary>
    public CountryName CountryName { get; private set; }

    /// <summary>
    /// Gets the Alpha-3 country code.
    /// </summary>
    public CountryAlpha3Code Alpha3Code { get; private set; }

    /// <summary>
    /// Creates a new country instance.
    /// </summary>
    /// <param name="countryName">Name of the country.</param>
    /// <param name="alpha3Code">Alpha-3 country code.</param>
    /// <returns>A new <see cref="Country"/> instance.</returns>
    public static Country Create(
        CountryName countryName,
        CountryAlpha3Code alpha3Code)
    {
        var country = new Country(
            Ulid.NewUlid(),
            countryName,
            alpha3Code);

        country.RaiseDomainEvent(new CountryCreatedDomainEvent(country.Id));
        return country;
    }
}
