using System;
using MediatR;
using FluentResults;

namespace KosHome.Application.Cities.GetCities;

/// <summary>
/// Represents the query to get a city by its unique identifier.
/// </summary>
/// <param name="CityId">The unique identifier of the city.</param>
public sealed record GetCityById(Ulid CityId) : IRequest<Result<CityResponse>>;