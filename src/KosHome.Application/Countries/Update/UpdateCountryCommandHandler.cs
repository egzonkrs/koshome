using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Domain.Common;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.ValueObjects.Countries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KosHome.Application.Countries.Update;

/// <summary>
/// Handler for the <see cref="UpdateCountryCommand"/>.
/// </summary>
public sealed class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, Result<bool>>
{
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateCountryCommandHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCountryCommandHandler"/> class.
    /// </summary>
    /// <param name="countryRepository">The country repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public UpdateCountryCommandHandler(ICountryRepository countryRepository, IUnitOfWork unitOfWork, ILogger<UpdateCountryCommandHandler> logger)
    {
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Handles the update country command.
    /// </summary>
    /// <param name="request">The update country command.</param>
    /// <returns>A result indicating success or failure.</returns>
    public async Task<Result<bool>> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        var existingCountry = await _countryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingCountry is null)
        {
            return Result.Fail(CountriesErrors.NotFound(request.Id.ToString()));
        }

        try
        {
            var countryName = new CountryName(request.CountryName);
            var alpha3Code = new CountryAlpha3Code(request.Alpha3Code);
            
            existingCountry.UpdateDetails(countryName, alpha3Code);
            
            await _countryRepository.UpdateAsync(existingCountry, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating Country with Id: {CountryId}", request.Id);
            return Result.Fail(CountriesErrors.UnexpectedError());
        }
    }
} 