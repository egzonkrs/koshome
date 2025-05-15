using System;
using KosHome.Domain.Abstractions;

namespace KosHome.Domain.Events.Countries;

/// <summary>
/// Domain event that gets raised when a country is updated.
/// </summary>
/// <param name="CountryId">The ID of the updated country.</param>
public sealed record CountryUpdatedDomainEvent(Ulid CountryId) : IDomainEvent; 