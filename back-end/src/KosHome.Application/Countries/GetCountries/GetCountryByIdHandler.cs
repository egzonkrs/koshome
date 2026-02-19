using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Application.Mappers;
using KosHome.Domain.Common;
using KosHome.Domain.Data.Repositories;
using MediatR;

namespace KosHome.Application.Countries.GetCountries;

/// <summary>
/// Handles the <see cref="GetCountryById"/> query.
/// </summary>
public sealed class GetCountryByIdHandler : IRequestHandler<GetCountryById, Result<CountryResponse>>
{
    private readonly ICountryRepository _countryRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCountryByIdHandler"/> class.
    /// </summary>
    /// <param name="countryRepository">The country repository.</param>
    public GetCountryByIdHandler(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    /// <summary>
    /// Handles the query to get a country by its ID.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the country response or an error.</returns>
    public async Task<Result<CountryResponse>> Handle(GetCountryById request, CancellationToken cancellationToken)
    {
        var country = await _countryRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (country is null)
        {
            return Result.Fail(CountriesErrors.NotFound(request.Id.ToString()));
        }
        
        return Result.Ok(country.ToResponse());
    }
} 