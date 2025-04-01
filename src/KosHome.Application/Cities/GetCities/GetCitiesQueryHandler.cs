using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Cities;
using KosHome.Domain.ValueObjects.Cities;
using MediatR;

namespace KosHome.Application.Cities.GetCities;

public class GetCitiesQueryHandler : IRequestHandler<GetCityById, Result<City>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetCitiesQueryHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<City>> Handle(GetCityById request, CancellationToken cancellationToken)
    {
        var city = await _cityRepository.GetByIdAsync(request.CityId, cancellationToken);
        return Result.Ok(city);
    }
}