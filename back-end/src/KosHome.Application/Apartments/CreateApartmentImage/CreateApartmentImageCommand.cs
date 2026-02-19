using System;
using System.Collections.Generic;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace KosHome.Application.Apartments.CreateApartmentImage;

/// <summary>
/// Command to create apartment images.
/// </summary>
public sealed class CreateApartmentImageCommand : IRequest<Result<List<Ulid>>>
{
    /// <summary>
    /// The apartment identifier.
    /// </summary>
    public Ulid ApartmentId { get; init; }
    
    /// <summary>
    /// The collection of images to create.
    /// </summary>
    public IEnumerable<IFormFile> Images { get; init; }
} 