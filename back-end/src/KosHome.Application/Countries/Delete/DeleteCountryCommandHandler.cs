using System;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Domain.Common;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Data.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace KosHome.Application.Countries.Delete;

/// <summary>
/// Handler for the <see cref="DeleteCountryCommand"/>.
/// </summary>
public sealed class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Result<bool>>
{
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteCountryCommandHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCountryCommandHandler"/> class.
    /// </summary>
    /// <param name="countryRepository">The country repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="logger">The logger.</param>
    public DeleteCountryCommandHandler(ICountryRepository countryRepository, IUnitOfWork unitOfWork, ILogger<DeleteCountryCommandHandler> logger)
    {
        _countryRepository = countryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Handles the delete country command.
    /// </summary>
    /// <param name="request">The delete country command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    public async Task<Result<bool>> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        var existingCountry = await _countryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingCountry is null)
        {
            return Result.Fail(CountriesErrors.NotFound(request.Id.ToString()));
        }

        try
        {
            return await _unitOfWork.ExecuteTransactionAsync(async scope =>
            {
                await _countryRepository.DeleteAsync(existingCountry, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                scope.Complete();
                return Result.Ok(true);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting Country with Id: {CountryId}", request.Id);
            return Result.Fail(CountriesErrors.UnexpectedError());
        }
    }
} 