using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Application.Abstractions.Pagination;
using KosHome.Application.Cities.Specifications;
using KosHome.Application.Mappers;
using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Data.Repositories;
using MediatR;

namespace KosHome.Application.Cities.GetCities;

public sealed class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, Result<PaginatedResult<CityResponse>>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IPaginationService _paginationService;

    public GetAllCitiesQueryHandler(
        ICityRepository cityRepository,
        IPaginationService paginationService)
    {
        _cityRepository = cityRepository;
        _paginationService = paginationService;
    }

    public async Task<Result<PaginatedResult<CityResponse>>> Handle(
        GetAllCitiesQuery request, 
        CancellationToken cancellationToken)
    {
        var specification = new CitiesPaginationSpecification(
            request.PaginationRequest, 
            request.CountryId);
            
        var result = await _paginationService.GetPaginatedAsync(
            _cityRepository, 
            specification, 
            cancellationToken);
        
        var isFailedResult = result.IsFailed;
        if (isFailedResult)
        {
            return Result.Fail(result.Errors);
        }

        var paginatedResponse = result.Value.Map(city => city.ToResponse());
        return Result.Ok(paginatedResponse);
    }
}
 