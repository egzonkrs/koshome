using System;
using MediatR;
using FluentResults;
using KosHome.Domain.Entities.Cities;

namespace KosHome.Application.Cities.GetCities;

public sealed class GetCityById : IRequest<Result<City>>
{
    /// <summary>
    /// The Id of the City.
    /// </summary>
    public Ulid CityId { get; set; }
}