using System;
using KosHome.Domain.Enums;

namespace KosHome.Application.Apartments.Common;

/// <summary>
/// Response model for apartment data.
/// </summary>
public sealed record ApartmentResponse
{
    /// <summary>
    /// The apartment identifier.
    /// </summary>
    public Ulid Id { get; init; }

    /// <summary>
    /// The apartment title.
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// The apartment description.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// The apartment price.
    /// </summary>
    public decimal Price { get; init; }

    /// <summary>
    /// The listing type.
    /// </summary>
    public string ListingType { get; init; } = string.Empty;

    /// <summary>
    /// The apartment address.
    /// </summary>
    public string Address { get; init; } = string.Empty;

    /// <summary>
    /// The city identifier.
    /// </summary>
    public Ulid CityId { get; init; }

    /// <summary>
    /// The number of bedrooms.
    /// </summary>
    public int Bedrooms { get; init; }

    /// <summary>
    /// The number of bathrooms.
    /// </summary>
    public int Bathrooms { get; init; }

    /// <summary>
    /// The square meters.
    /// </summary>
    public int SquareMeters { get; init; }

    /// <summary>
    /// The latitude coordinate.
    /// </summary>
    public double Latitude { get; init; }

    /// <summary>
    /// The longitude coordinate.
    /// </summary>
    public double Longitude { get; init; }

    /// <summary>
    /// The creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The last update timestamp.
    /// </summary>
    public DateTime UpdatedAt { get; init; }
}
