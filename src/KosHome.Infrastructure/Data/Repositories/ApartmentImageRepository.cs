using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.ApartmentImages;
using KosHome.Infrastructure.Data.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace KosHome.Infrastructure.Data.Repositories;

/// <summary>
/// Repository for apartment image entities.
/// </summary>
public sealed class ApartmentImageRepository : EfCoreRepository<ApartmentImage>, IApartmentImageRepository
{
    /// <summary>
    /// Initializes a new instance of the ApartmentImageRepository class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public ApartmentImageRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    /// <summary>
    /// Gets all apartment images for a specific apartment.
    /// </summary>
    /// <param name="apartmentId">The apartment identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of apartment images ordered by primary status and creation date.</returns>
    public async Task<IReadOnlyList<ApartmentImage>> GetByApartmentIdAsync(Ulid apartmentId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(ai => ai.ApartmentId == apartmentId)
            .OrderByDescending(ai => ai.IsPrimary)
            .ThenBy(ai => ai.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the primary image for a specific apartment.
    /// </summary>
    /// <param name="apartmentId">The apartment identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The primary apartment image or null if not found.</returns>
    public async Task<ApartmentImage> GetPrimaryImageByApartmentIdAsync(Ulid apartmentId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(ai => ai.ApartmentId == apartmentId && ai.IsPrimary)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Sets the primary status of an apartment image.
    /// </summary>
    /// <param name="apartmentImageId">The apartment image identifier.</param>
    /// <param name="isPrimary">Whether the image should be set as primary.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
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
            var otherImages = await DbSet
                .Where(ai => ai.ApartmentId == image.ApartmentId && ai.Id != apartmentImageId)
                .ToListAsync(cancellationToken);

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