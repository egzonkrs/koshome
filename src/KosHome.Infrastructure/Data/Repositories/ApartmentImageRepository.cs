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
public sealed class ApartmentImageRepository : EfRepositoryBase<ApartmentImage>, IApartmentImageRepository
{
    private readonly DbSet<ApartmentImage> _dbSet;
    
    public ApartmentImageRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<ApartmentImage>();
    }

    public async Task<IReadOnlyList<ApartmentImage>> GetByApartmentIdAsync(Ulid apartmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ai => ai.ApartmentId == apartmentId)
            .OrderByDescending(ai => ai.IsPrimary)
            .ThenBy(ai => ai.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<ApartmentImage> GetPrimaryImageByApartmentIdAsync(Ulid apartmentId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(ai => ai.ApartmentId == apartmentId && ai.IsPrimary)
            .FirstOrDefaultAsync(cancellationToken);
    }

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
            var otherImages = await _dbSet
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