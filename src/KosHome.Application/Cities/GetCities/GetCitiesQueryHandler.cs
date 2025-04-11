using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Application.Mappers;
using KosHome.Domain.Common;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Data.Repositories;
using MediatR;

namespace KosHome.Application.Cities.GetCities;

public class GetCitiesQueryHandler : IRequestHandler<GetCityById, Result<CityResponse>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetCitiesQueryHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<CityResponse>> Handle(GetCityById request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.CityId, cancellationToken);
        throw new Exception();
        return city is null 
            ? Result.Fail(CitiesErrors.NotFound(id: request.CityId.ToString())).WithError(CitiesErrors.UnexpectedError())
            : Result.Ok(city.ToResponse());
    }
}