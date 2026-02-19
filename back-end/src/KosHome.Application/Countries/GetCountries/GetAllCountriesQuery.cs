using System.Collections.Generic;
using FluentResults;
using MediatR;

namespace KosHome.Application.Countries.GetCountries;

/// <summary>
/// Query to get all countries.
/// </summary>
public sealed class GetAllCountriesQuery : IRequest<Result<IEnumerable<CountryResponse>>>
{
} 