using System;
using Ardalis.Specification;
using KosHome.Domain.Entities.ApartmentImages;

namespace KosHome.Application.ApartmentImages.Specifications;

/// <summary>
/// Specification for finding the primary apartment image by apartment ID.
/// </summary>
public sealed class PrimaryApartmentImageByApartmentIdSpecification : Specification<ApartmentImage>, ISingleResultSpecification<ApartmentImage>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimaryApartmentImageByApartmentIdSpecification"/> class.
    /// </summary>
    /// <param name="apartmentId">The apartment ID to filter by.</param>
    public PrimaryApartmentImageByApartmentIdSpecification(Ulid apartmentId)
    {
        Query.Where(image => image.ApartmentId == apartmentId && image.IsPrimary);
    }
}
