using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Domain.Common;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.ValueObjects.Cities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KosHome.Application.Cities.Update;

/// <summary>
/// Handler for the <see cref="UpdateCityCommand"/>.
/// </summary>
public sealed class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, Result<bool>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCityCommandHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCityCommandHandler"/> class.
    /// </summary>
    /// <param name="cityRepository">The city repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public UpdateCityCommandHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork, ILogger<UpdateCityCommandHandler> logger)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Handles the update city command.
    /// </summary>
    /// <param name="request">The update city command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    public async Task<Result<bool>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        var existingCity = await _cityRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingCity is null)
        {
            return Result.Fail(CitiesErrors.NotFound(request.Id.ToString()));
        }

        try
        {
            var cityName = new CityName(request.CityName);
            var cityAlpha3Code = new CityAlpha3Code(request.Alpha3Code);
            
            existingCity.UpdateDetails(cityName, cityAlpha3Code, request.CountryId);
            
            await _cityRepository.UpdateAsync(existingCity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Ok(true);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Error while updating City with Id: {CityId}", request.Id);
            return Result.Fail(CitiesErrors.UnexpectedError());
        }
    }
} 