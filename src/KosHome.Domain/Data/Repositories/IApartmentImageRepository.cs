using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using KosHome.Domain.Entities.ApartmentImages;

namespace KosHome.Domain.Data.Repositories;

/// <summary>
/// Interface for the repository handling apartment image entities.
/// </summary>
public interface IApartmentImageRepository : IRepositoryBase<ApartmentImage>
{
    /// <summary>
    /// Gets all images for a specific apartment.
    /// </summary>
    /// <param name="apartmentId">The apartment identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of apartment images.</returns>
    Task<IReadOnlyList<ApartmentImage>> GetByApartmentIdAsync(Ulid apartmentId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the primary image for a specific apartment.
    /// </summary>
    /// <param name="apartmentId">The apartment identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The primary apartment image if found, null otherwise.</returns>
    Task<ApartmentImage> GetPrimaryImageByApartmentIdAsync(Ulid apartmentId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sets the primary status of an apartment image.
    /// </summary>
    /// <param name="apartmentImageId">The apartment image identifier.</param>
    /// <param name="isPrimary">The primary status value.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetPrimaryStatusAsync(Ulid apartmentImageId, bool isPrimary, CancellationToken cancellationToken = default);
} 