using System;
using KosHome.Domain.Abstractions;

namespace KosHome.Domain.Events.Countries;

/// <summary>
/// Domain event that gets raised when a country is created.
/// </summary>
/// <param name="CountryId">The ID of the created country.</param>
public sealed record CountryCreatedDomainEvent(Ulid CountryId) : IDomainEvent;