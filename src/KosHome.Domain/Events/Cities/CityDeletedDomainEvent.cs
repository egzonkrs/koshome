using System;
using KosHome.Domain.Abstractions;

namespace KosHome.Domain.Events.Cities;

/// <summary>
/// Domain event that gets raised when a city is deleted.
/// </summary>
/// <param name="CityId">The ID of the deleted city.</param>
public sealed record CityDeletedDomainEvent(Ulid CityId) : IDomainEvent; 