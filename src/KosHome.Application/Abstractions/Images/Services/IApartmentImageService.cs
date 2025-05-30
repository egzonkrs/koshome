using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.AspNetCore.Http;

namespace KosHome.Application.Abstractions.Images.Services;

/// <summary>
/// Interface for a service that processes apartment images.
/// </summary>
public interface IApartmentImageService
{
    /// <summary>
    /// Processes and saves an apartment image.
    /// </summary>
    /// <param name="apartmentId">The apartment identifier.</param>
    /// <param name="image">The image file to process.</param>
    /// <param name="isPrimary">Whether the image is a primary image.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the image path if successful.</returns>
    Task<Result<string>> ProcessApartmentImageAsync(Ulid apartmentId, IFormFile image, bool isPrimary, CancellationToken cancellationToken);
    
    /// <summary>
    /// Processes and saves multiple apartment images.
    /// </summary>
    /// <param name="apartmentId">The apartment identifier.</param>
    /// <param name="images">The collection of image files to process.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the collection of image paths if successful.</returns>
    Task<Result<List<string>>> ProcessApartmentImagesAsync(Ulid apartmentId, IEnumerable<IFormFile> images, CancellationToken cancellationToken);
} 