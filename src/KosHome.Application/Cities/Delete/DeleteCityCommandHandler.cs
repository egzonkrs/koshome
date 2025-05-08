using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Domain.Common;
using KosHome.Domain.Data.Abstractions;
using KosHome.Domain.Data.Repositories;
using MediatR;

namespace KosHome.Application.Cities.Delete;

/// <summary>
/// Handler for the <see cref="DeleteCityCommand"/>.
/// </summary>
public sealed class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, Result<bool>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCityCommandHandler"/> class.
    /// </summary>
    /// <param name="cityRepository">The city repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public DeleteCityCommandHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork)
    {
        _cityRepository = cityRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the command to delete a city.
    /// </summary>
    /// <param name="request">The delete city command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    public async Task<Result<bool>> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.ExecuteTransactionAsync(async scope =>
        {
            var city = await _cityRepository.GetByIdAsync(request.Id, cancellationToken);
            if (city is null)
            {
                return Result.Fail(CitiesErrors.NotFound(request.Id.ToString()));
            }

            await _cityRepository.DeleteAsync(city, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
            return Result.Ok();
        });
    }
} 