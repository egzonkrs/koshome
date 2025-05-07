using System;
using KosHome.Domain.Abstractions;
using KosHome.Domain.Data.Abstractions;

namespace KosHome.Domain.Entities.PropertyTypes;

/// <summary>
/// Represents a property type entity (e.g., Apartment, House).
/// </summary>
public sealed class PropertyType : DomainEntity, IEntity<Ulid>
{
    private PropertyType(Ulid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    private PropertyType()
    {
    }

    /// <summary>
    /// Gets the property type identifier.
    /// </summary>
    public Ulid Id { get; set; }

    /// <summary>
    /// Gets the name of the property type.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the description of the property type.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Gets the creation date and time.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the last updated date and time.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Creates a new property type instance.
    /// </summary>
    /// <param name="name">The name of the property type.</param>
    /// <param name="description">The description of the property type.</param>
    /// <returns>A new <see cref="PropertyType"/> instance.</returns>
    public static PropertyType Create(string name, string description)
    {
        return new PropertyType(Ulid.NewUlid(), name, description);
    }

    /// <summary>
    /// Updates the property type details.
    /// </summary>
    /// <param name="name">The updated name.</param>
    /// <param name="description">The updated description.</param>
    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
} 