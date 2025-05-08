using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Application.Mappers;
using KosHome.Domain.Data.Repositories;
using MediatR;

namespace KosHome.Application.Cities.GetCities;

/// <summary>
/// Handles the <see cref="GetAllCitiesQuery"/>.
/// </summary>
public sealed class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, Result<IEnumerable<CityResponse>>>
{
    private readonly ICityRepository _cityRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllCitiesQueryHandler"/> class.
    /// </summary>
    /// <param name="cityRepository">The city repository.</param>
    public GetAllCitiesQueryHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    /// <summary>
    /// Handles the query to get all cities.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing a list of city responses or an error.</returns>
    public async Task<Result<IEnumerable<CityResponse>>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        var cities = await _cityRepository.GetAllAsync(cancellationToken: cancellationToken);
        var cityResponses = cities.Select(city => city.ToResponse());
        return Result.Ok(cityResponses);
    }
} 