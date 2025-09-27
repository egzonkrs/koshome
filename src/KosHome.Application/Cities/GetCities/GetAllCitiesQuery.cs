using System;
using FluentResults;
using KosHome.Domain.Common.Pagination;
using MediatR;

namespace KosHome.Application.Cities.GetCities;

/// <summary>
/// Query for getting cities with pagination support.
/// </summary>
public sealed record GetAllCitiesQuery : IRequest<Result<PaginatedResult<CityResponse>>>
{
    /// <summary>
    /// The pagination request.
    /// </summary>
    public PaginationRequest PaginationRequest { get; init; } = new();

    /// <summary>
    /// Optional country ID filter.
    /// </summary>
    public Ulid? CountryId { get; init; }

    /// <summary>
    /// Optional search term for city name.
    /// </summary>
    public string? SearchTerm { get; init; }
} 