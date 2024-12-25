using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Cities;
using MediatR;

namespace KosHome.Application.Cities.GetCities;

public class GetCitiesQueryHandler : IRequestHandler<GetCityById, Result<City>>
{
    private readonly ICityRepository _cityRepository;

    public GetCitiesQueryHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }
    
    public async Task<Result<City>> Handle(GetCityById request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.CityId, cancellationToken);
        return Result.Ok(city);
    }
}