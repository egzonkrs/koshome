using System;
using KosHome.Domain.Abstractions;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Events.ApartmentImages;
using KosHome.Domain.ValueObjects.ApartmentImages;

namespace KosHome.Domain.Entities.ApartmentImages;

/// <summary>
/// Represents an image entity associated with an apartment.
/// </summary>
public sealed class ApartmentImage : DomainEntity, IEntity<Ulid>
{
    private ApartmentImage(
        Ulid id,
        Ulid apartmentId,
        ImageUrl imageUrl,
        bool isPrimary)
    {
        Id = id;
        ApartmentId = apartmentId;
        ImageUrl = imageUrl;
        IsPrimary = isPrimary;
        CreatedAt = DateTime.UtcNow;
    }

    private ApartmentImage()
    {
    }

    /// <summary>
    /// The unique identifier of the apartment image.
    /// </summary>
    public Ulid Id { get; set; }
    
    /// <summary>
    /// Gets the apartment identifier.
    /// </summary>
    public Ulid ApartmentId { get; private set; }

    /// <summary>
    /// Gets the image URL.
    /// </summary>
    public ImageUrl ImageUrl { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the image is primary.
    /// </summary>
    public bool IsPrimary { get; private set; }

    /// <summary>
    /// Gets the creation date and time.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Creates a new image instance.
    /// </summary>
    /// <param name="apartmentId">Apartment identifier.</param>
    /// <param name="imageUrl">Image URL.</param>
    /// <param name="isPrimary">Indicates if the image is primary.</param>
    /// <returns>A new <see cref="ApartmentImage"/> instance.</returns>
    public static ApartmentImage Create(Ulid apartmentId, ImageUrl imageUrl, bool isPrimary)
    {
        var image = new ApartmentImage(Ulid.NewUlid(), apartmentId, imageUrl, isPrimary);

        image.RaiseDomainEvent(new ApartmentImageAddedDomainEvent(image.Id));
        return image;
    }
}