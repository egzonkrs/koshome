using System;
using FluentResults;
using MediatR;

namespace KosHome.Application.Countries.GetCountries;

/// <summary>
/// Query to get a country by its ID.
/// </summary>
public sealed class GetCountryById : IRequest<Result<CountryResponse>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetCountryById"/> class.
    /// </summary>
    /// <param name="id">The ID of the country to retrieve.</param>
    public GetCountryById(Ulid id)
    {
        Id = id;
    }
    
    /// <summary>
    /// Gets the ID of the country to retrieve.
    /// </summary>
    public Ulid Id { get; }
} 