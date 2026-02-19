using KosHome.Application.Apartments.Common;
using KosHome.Domain.Entities.Apartments;

namespace KosHome.Application.Mappers;

/// <summary>
/// Mapper for apartment entities to response models.
/// </summary>
public static class ApartmentMapper
{
    /// <summary>
    /// Converts an apartment entity to a response model.
    /// </summary>
    /// <param name="apartment">The apartment entity.</param>
    /// <returns>The apartment response model.</returns>
    public static ApartmentResponse ToResponse(this Apartment apartment)
    {
        return new ApartmentResponse
        {
            Id = apartment.Id,
            Title = apartment.Title.Value,
            Description = apartment.Description.Value,
            Price = apartment.Price.Value,
            ListingType = apartment.ListingType.ToString(),
            Address = apartment.Address.Value,
            CityId = apartment.CityId,
            Bedrooms = apartment.Bedrooms,
            Bathrooms = apartment.Bathrooms,
            SquareMeters = apartment.SquareMeters,
            Latitude = apartment.Latitude,
            Longitude = apartment.Longitude,
            CreatedAt = apartment.CreatedAt,
            UpdatedAt = apartment.UpdatedAt
        };
    }
}
