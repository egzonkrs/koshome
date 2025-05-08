using System.Collections.Generic;
using FluentResults;
using MediatR;

namespace KosHome.Application.Cities.GetCities;

/// <summary>
/// Represents the query to get all cities.
/// </summary>
public sealed record GetAllCitiesQuery : IRequest<Result<IEnumerable<CityResponse>>>; 