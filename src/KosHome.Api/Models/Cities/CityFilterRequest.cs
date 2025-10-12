using System;
using KosHome.Api.Models.Common;

namespace KosHome.Api.Models.Cities;

public sealed record CityFilterRequest : PaginationRequest
{
    public Ulid? CountryId { get; init; }
}

