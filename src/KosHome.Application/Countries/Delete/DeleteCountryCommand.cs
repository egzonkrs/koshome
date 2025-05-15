using System;
using FluentResults;
using MediatR;

namespace KosHome.Application.Countries.Delete;

/// <summary>
/// Command to delete a country.
/// </summary>
public sealed class DeleteCountryCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCountryCommand"/> class.
    /// </summary>
    /// <param name="id">The ID of the country to delete.</param>
    public DeleteCountryCommand(Ulid id)
    {
        Id = id;
    }
    
    /// <summary>
    /// Gets the ID of the country to delete.
    /// </summary>
    public Ulid Id { get; }
} 