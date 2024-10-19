using System;
using KosHome.Domain.Abstractions;

namespace KosHome.Domain.Events.Countries;

public sealed record CountryCreatedDomainEvent(Ulid CountryId) : IDomainEvent;