using System;
using FluentResults;
using MediatR;

namespace KosHome.Application.Countries.Update;

/// <summary>
/// Command to update an existing country.
/// </summary>
public sealed class UpdateCountryCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// The ID of the country to update.
    /// </summary>
    public Ulid Id { get; set; }
    
    /// <summary>
    /// The updated name of the country.
    /// </summary>
    public string CountryName { get; set; }

    /// <summary>
    /// The updated alpha-3 code of the country.
    /// </summary>
    public string Alpha3Code { get; set; }
} 