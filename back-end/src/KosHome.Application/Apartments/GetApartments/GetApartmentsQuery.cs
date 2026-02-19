using System;
using FluentResults;
using KosHome.Application.Apartments.Common;
using KosHome.Domain.Common.Pagination;
using MediatR;

namespace KosHome.Application.Apartments.GetApartments;

public sealed record GetApartmentsQuery : IRequest<Result<PaginatedResult<ApartmentResponse>>>
{
    public PaginationRequest PaginationRequest { get; init; } = new();

    public Ulid? CityId { get; init; }

    public decimal? MinPrice { get; init; }

    public decimal? MaxPrice { get; init; }
}