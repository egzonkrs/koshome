using System;
using KosHome.Domain.Abstractions;

namespace KosHome.Domain.Events.Cities;

public sealed record CityCreatedDomainEvent(Ulid CityId) : IDomainEvent;