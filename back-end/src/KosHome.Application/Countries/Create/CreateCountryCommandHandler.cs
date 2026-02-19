using System;
using MediatR;
using FluentResults;
using System.Threading;
using System.Threading.Tasks;
using KosHome.Domain.Common;
using KosHome.Domain.Abstractions;
using Microsoft.Extensions.Logging;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Data.Repositories;
using KosHome.Domain.Entities.Countries;
using KosHome.Domain.ValueObjects.Countries;

namespace KosHome.Application.Countries.Create;

/// <summary>
/// Handler for the <see cref="CreateCountryCommand"/>.
/// </summary>
public sealed class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Result<Ulid>>
{
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateCountryCommandHandler> _logger;
    private readonly IUserContextAccessor _userContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCountryCommandHandler"/> class.
    /// </summary>
    /// <param name="countryRepository">The country repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="userContextAccessor">The user context accessor.</param>
    public CreateCountryCommandHandler(
        ICountryRepository countryRepository, 
        IUnitOfWork unitOfWork, 
        ILogger<CreateCountryCommandHandler> logger, 
        IUserContextAccessor userContextAccessor)
    {
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _userContextAccessor = userContextAccessor;
    }

    /// <summary>
    /// Handles the create country command.
    /// </summary>
    /// <param name="request">The create country command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the ID of the newly created country.</returns>
    public async Task<Result<Ulid>> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var countryName = new CountryName(request.CountryName);
            var alpha3Code = new CountryAlpha3Code(request.Alpha3Code);
            
            var country = Country.Create(countryName, alpha3Code);
            
            await _countryRepository.AddAsync(country, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Ok(country.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while creating a new Country");
            return Result.Fail(CountriesErrors.UnexpectedError());
        }
    }
} 