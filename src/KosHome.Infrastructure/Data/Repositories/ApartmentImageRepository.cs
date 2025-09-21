using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification.EntityFrameworkCore;
using KosHome.Application.ApartmentImages.Specifications;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.ApartmentImages;

namespace KosHome.Infrastructure.Data.Repositories;

/// <summary>
/// Apartment image repository implementation using Ardalis.Specification.
/// </summary>
internal sealed class ApartmentImageRepository : RepositoryBase<ApartmentImage>, IApartmentImageRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApartmentImageRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public ApartmentImageRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ApartmentImage>> GetByApartmentIdAsync(Ulid apartmentId, CancellationToken cancellationToken = default)
    {
        var specification = new ApartmentImagesByApartmentIdSpecification(apartmentId);
        var results = await ListAsync(specification, cancellationToken);
        return results.AsReadOnly();
    }

    /// <inheritdoc />
    public async Task<ApartmentImage> GetPrimaryImageByApartmentIdAsync(Ulid apartmentId, CancellationToken cancellationToken = default)
    {
        var specification = new PrimaryApartmentImageByApartmentIdSpecification(apartmentId);
        return await FirstOrDefaultAsync(specification, cancellationToken);
    }

    /// <inheritdoc />
    public async Task SetPrimaryStatusAsync(Ulid apartmentImageId, bool isPrimary, CancellationToken cancellationToken = default)
    {
        var image = await GetByIdAsync(apartmentImageId, cancellationToken);
        if (image is null)
        {
            return;
        }

        // If setting to primary, first reset all other images from the same apartment
        if (isPrimary)
        {
            var otherImagesSpec = new NonPrimaryApartmentImagesByApartmentIdSpecification(image.ApartmentId, apartmentImageId);
            var otherImages = await ListAsync(otherImagesSpec, cancellationToken);

            foreach (var otherImage in otherImages)
            {
                if (otherImage.IsPrimary)
                {
                    otherImage.SetPrimaryStatus(false);
                }
            }
        }

        // Update the primary status of the target image
        image.SetPrimaryStatus(isPrimary);
    }

} 