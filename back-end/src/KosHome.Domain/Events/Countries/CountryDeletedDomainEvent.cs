using System;
using KosHome.Domain.Abstractions;

namespace KosHome.Domain.Events.Countries;

/// <summary>
/// Domain event that gets raised when a country is deleted.
/// </summary>
/// <param name="CountryId">The ID of the deleted country.</param>
public sealed record CountryDeletedDomainEvent(Ulid CountryId) : IDomainEvent; 