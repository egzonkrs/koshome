using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Application.Mappers;
using KosHome.Domain.Data.Repositories;
using MediatR;

namespace KosHome.Application.Countries.GetCountries;

/// <summary>
/// Handles the <see cref="GetAllCountriesQuery"/>.
/// </summary>
public sealed class GetAllCountriesQueryHandler : IRequestHandler<GetAllCountriesQuery, Result<IEnumerable<CountryResponse>>>
{
    private readonly ICountryRepository _countryRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllCountriesQueryHandler"/> class.
    /// </summary>
    /// <param name="countryRepository">The country repository.</param>
    public GetAllCountriesQueryHandler(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    /// <summary>
    /// Handles the query to get all countries.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing a list of country responses or an error.</returns>
    public async Task<Result<IEnumerable<CountryResponse>>> Handle(GetAllCountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = await _countryRepository.GetAllAsync(cancellationToken: cancellationToken);
        var countryResponses = countries.Select(country => country.ToResponse());
        return Result.Ok(countryResponses);
    }
} 