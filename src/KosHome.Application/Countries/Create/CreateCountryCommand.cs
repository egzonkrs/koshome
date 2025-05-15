using System;
using FluentResults;
using MediatR;

namespace KosHome.Application.Countries.Create;

/// <summary>
/// Command to create a new country.
/// </summary>
public sealed class CreateCountryCommand : IRequest<Result<Ulid>>
{
    /// <summary>
    /// The name of the country.
    /// </summary>
    public string CountryName { get; set; }

    /// <summary>
    /// The alpha-3 code of the country.
    /// </summary>
    public string Alpha3Code { get; set; }
} 