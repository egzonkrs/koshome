using System;
using KosHome.Domain.Abstractions;

namespace KosHome.Domain.Events.Cities;

/// <summary>
/// Domain event that gets raised when a city is updated.
/// </summary>
/// <param name="CityId">The ID of the updated city.</param>
public sealed record CityUpdatedDomainEvent(Ulid CityId) : IDomainEvent; 