using System;
using KosHome.Api.Models.Common;

namespace KosHome.Api.Models.Apartments;

public sealed record ApartmentFilterRequest : PaginationRequest
{
    public Ulid? CityId { get; init; }
    
    public decimal? MinPrice { get; init; }
    
    public decimal? MaxPrice { get; init; }
}

