using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Cities;
using KosHome.Domain.ValueObjects.Cities;
using MediatR;

namespace KosHome.Application.Cities.Create;

public sealed class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, Result<Ulid>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCityCommandHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Ulid>> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var cityName = new CityName(request.CityName);
        var cityAlpha3Code = new CityAlpha3Code(request.Alpha3Code);

        var city = City.Create(
            cityName,
            cityAlpha3Code,
            request.CountryId);

        await _cityRepository.InsertAsync(city, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok(city.Id);
    }
}
