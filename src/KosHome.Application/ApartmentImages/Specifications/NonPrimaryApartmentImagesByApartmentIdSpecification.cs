using System;
using Ardalis.Specification;
using KosHome.Domain.Entities.ApartmentImages;

namespace KosHome.Application.ApartmentImages.Specifications;

/// <summary>
/// Specification for getting non-primary apartment images by apartment ID.
/// </summary>
public sealed class NonPrimaryApartmentImagesByApartmentIdSpecification : Specification<ApartmentImage>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NonPrimaryApartmentImagesByApartmentIdSpecification"/> class.
    /// </summary>
    /// <param name="apartmentId">The apartment ID to filter by.</param>
    /// <param name="excludeImageId">Optional image ID to exclude from the results.</param>
    public NonPrimaryApartmentImagesByApartmentIdSpecification(Ulid apartmentId, Ulid? excludeImageId = null)
    {
        Query.Where(image => image.ApartmentId == apartmentId);
        
        if (excludeImageId.HasValue)
        {
            Query.Where(image => image.Id != excludeImageId.Value);
        }
    }
}
