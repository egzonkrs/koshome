using System;
using KosHome.Domain.Abstractions;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Enums;
using KosHome.Domain.Entities.PropertyTypes;
using KosHome.Domain.Events.Apartments;
using KosHome.Domain.ValueObjects.Apartments;

namespace KosHome.Domain.Entities.Apartments;

/// <summary>
/// Represents an apartment entity.
/// </summary>
public sealed class Apartment : DomainEntity, IEntity<Ulid>
{
    private Apartment(
        Ulid id,
        Ulid userId,
        Title title,
        Description description,
        Price price,
        ListingType listingType,
        Ulid propertyTypeId,
        Address address,
        Ulid locationId,
        int bedrooms,
        int bathrooms,
        int squareMeters,
        double latitude,
        double longitude
        )
    {
        Id = id;
        UserId = userId;
        Title = title;
        Description = description;
        Price = price;
        ListingType = listingType;
        PropertyTypeId = propertyTypeId;
        Address = address;
        LocationId = locationId;
        Bedrooms = bedrooms;
        Bathrooms = bathrooms;
        SquareMeters = squareMeters;
        Latitude = latitude;
        Longitude = longitude;
        
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    private Apartment()
    {
    }
    
    /// <summary>
    /// The Id of the apartment.
    /// </summary>
    public Ulid Id { get; set; }

    /// <summary>
    /// Gets the user identifier who owns the apartment.
    /// </summary>
    public Ulid UserId { get; private set; }

    /// <summary>
    /// Gets the title of the apartment.
    /// </summary>
    public Title Title { get; private set; }

    /// <summary>
    /// Gets the description of the apartment.
    /// </summary>
    public Description Description { get; private set; }

    /// <summary>
    /// Gets the price of the apartment.
    /// </summary>
    public Price Price { get; private set; }

    /// <summary>
    /// Gets the listing type (Sale or Rent).
    /// </summary>
    public ListingType ListingType { get; private set; }

    /// <summary>
    /// Gets the property type ID.
    /// </summary>
    public Ulid PropertyTypeId { get; private set; }
    
    /// <summary>
    /// Gets the property type navigation property.
    /// </summary>
    public PropertyType PropertyType { get; private set; }

    /// <summary>
    /// Gets the address of the apartment.
    /// </summary>
    public Address Address { get; private set; }

    /// <summary>
    /// Gets the location identifier.
    /// </summary>
    public Ulid LocationId { get; private set; }

    /// <summary>
    /// Gets the number of bedrooms.
    /// </summary>
    public int Bedrooms { get; private set; }

    /// <summary>
    /// Gets the number of bathrooms.
    /// </summary>
    public int Bathrooms { get; private set; }

    /// <summary>
    /// Gets the size in square meters.
    /// </summary>
    public int SquareMeters { get; private set; }
    
    /// <summary>
    /// Gets the latitude of the apartment's location.
    /// </summary>
    public double Latitude { get; private set; }

    /// <summary>
    /// Gets the longitude of the apartment's location.
    /// </summary>
    public double Longitude { get; private set; }

    /// <summary>
    /// Gets the creation date and time.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the last updated date and time.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Creates a new apartment instance.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="title">Title of the apartment.</param>
    /// <param name="description">Description of the apartment.</param>
    /// <param name="price">Price of the apartment.</param>
    /// <param name="listingType">Listing type (Sale or Rent).</param>
    /// <param name="propertyTypeId">Property type identifier.</param>
    /// <param name="address">Address of the apartment.</param>
    /// <param name="locationId">Location identifier.</param>
    /// <param name="bedrooms">Number of bedrooms.</param>
    /// <param name="bathrooms">Number of bathrooms.</param>
    /// <param name="squareMeters">Size in square meters.</param>
    /// <param name="latitude">Latitude coordinate.</param>
    /// <param name="longitude">Longitude coordinate.</param>
    /// <returns>A new <see cref="Apartment"/> instance.</returns>
    public static Apartment Create(
        Ulid userId,
        Title title,
        Description description,
        Price price,
        ListingType listingType,
        Ulid propertyTypeId,
        Address address,
        Ulid locationId,
        int bedrooms,
        int bathrooms,
        int squareMeters,
        double latitude,
        double longitude)
    {
        var apartment = new Apartment(
            Ulid.NewUlid(),
            userId,
            title,
            description,
            price,
            listingType,
            propertyTypeId,
            address,
            locationId,
            bedrooms,
            bathrooms,
            squareMeters,
            latitude,
            longitude);

        apartment.RaiseDomainEvent(new ApartmentCreatedDomainEvent(apartment.Id));
        return apartment;
    }

    /// <summary>
    /// Updates the apartment details.
    /// </summary>
    /// <param name="title">New title.</param>
    /// <param name="description">New description.</param>
    /// <param name="price">New price.</param>
    public void UpdateDetails(Title title, Description description, Price price)
    {
        Title = title;
        Description = description;
        Price = price;
        UpdatedAt = DateTime.UtcNow;
        RaiseDomainEvent(new ApartmentUpdatedDomainEvent(Id));
    }
}
