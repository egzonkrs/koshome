using System;
using Ardalis.Specification;
using KosHome.Domain.Entities.ApartmentImages;

namespace KosHome.Application.ApartmentImages.Specifications;

/// <summary>
/// Specification for getting apartment images by apartment ID, ordered by primary status and creation date.
/// </summary>
public sealed class ApartmentImagesByApartmentIdSpecification : Specification<ApartmentImage>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApartmentImagesByApartmentIdSpecification"/> class.
    /// </summary>
    /// <param name="apartmentId">The apartment ID to filter by.</param>
    public ApartmentImagesByApartmentIdSpecification(Ulid apartmentId)
    {
        Query.Where(image => image.ApartmentId == apartmentId)
             .OrderByDescending(image => image.IsPrimary)
             .ThenBy(image => image.CreatedAt);
    }
}
