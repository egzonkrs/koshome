using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Application.Cities.Specifications;
using KosHome.Application.Mappers;
using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Data.Repositories;
using MediatR;

namespace KosHome.Application.Cities.GetCities;

/// <summary>
/// Handles the <see cref="GetAllCitiesQuery"/> with enhanced pagination support.
/// </summary>
public sealed class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, Result<PaginatedResult<CityResponse>>>
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
    /// Handles the query to get all cities with pagination and filtering.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing a paginated list of city responses.</returns>
    public async Task<Result<PaginatedResult<CityResponse>>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        var specification = new CitiesPaginationSpecification(
            request.PaginationRequest, 
            request.CountryId, 
            request.SearchTerm);
            
        var paginatedCities = await _cityRepository.GetPaginatedAsync(specification, cancellationToken);
        var paginatedResponse = paginatedCities.Map(city => city.ToResponse());
        return Result.Ok(paginatedResponse);
    }
} 