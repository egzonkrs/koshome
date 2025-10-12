using System;
using FluentResults;
using KosHome.Domain.Common.Pagination;
using MediatR;

namespace KosHome.Application.Cities.GetCities;

public sealed record GetAllCitiesQuery : IRequest<Result<PaginatedResult<CityResponse>>>
{
    public PaginationRequest PaginationRequest { get; init; } = new();

    public Ulid? CountryId { get; init; }
}
 