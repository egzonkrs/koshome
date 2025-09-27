using System;
using FluentResults;
using KosHome.Application.Apartments.Common;
using KosHome.Domain.Common.Pagination;
using MediatR;

namespace KosHome.Application.Apartments.GetApartments;

/// <summary>
/// Query for getting apartments with pagination and filtering support.
/// </summary>
public sealed record GetApartmentsQuery : IRequest<Result<PaginatedResult<ApartmentResponse>>>
{
    /// <summary>
    /// The pagination request.
    /// </summary>
    public PaginationRequest PaginationRequest { get; init; } = new();

    /// <summary>
    /// Optional city ID filter.
    /// </summary>
    public Ulid? CityId { get; init; }

    /// <summary>
    /// Optional minimum price filter.
    /// </summary>
    public decimal? MinPrice { get; init; }

    /// <summary>
    /// Optional maximum price filter.
    /// </summary>
    public decimal? MaxPrice { get; init; }

    /// <summary>
    /// Optional search term for title or description.
    /// </summary>
    public string? SearchTerm { get; init; }
}