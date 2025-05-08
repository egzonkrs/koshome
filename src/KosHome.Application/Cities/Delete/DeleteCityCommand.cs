using System;
using FluentResults;
using MediatR;

namespace KosHome.Application.Cities.Delete;

/// <summary>
/// Command to delete a city.
/// </summary>
/// <param name="Id">The ID of the city to delete.</param>
public sealed record DeleteCityCommand(Ulid Id) : IRequest<Result<bool>>; 