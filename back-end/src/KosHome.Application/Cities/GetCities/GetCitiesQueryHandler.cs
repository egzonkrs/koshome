using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Application.Mappers;
using KosHome.Domain.Common;
using KosHome.Domain.Data.Repositories;
using MediatR;

namespace KosHome.Application.Cities.GetCities;

public class GetCitiesQueryHandler : IRequestHandler<GetCityById, Result<CityResponse>>
{
    private readonly ICityRepository _cityRepository;

    public GetCitiesQueryHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }
    
    public async Task<Result<CityResponse>> Handle(GetCityById request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.CityId, cancellationToken);
        return city is null 
            ? Result.Fail(CitiesErrors.NotFound(id: request.CityId.ToString())).WithError(CitiesErrors.UnexpectedError())
            : Result.Ok(city.ToResponse());
    }
}