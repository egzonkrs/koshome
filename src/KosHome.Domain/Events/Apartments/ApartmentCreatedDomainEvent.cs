using System;
using KosHome.Domain.Abstractions;

namespace KosHome.Domain.Events.Apartments;

public sealed record ApartmentCreatedDomainEvent(Ulid UserId) : IDomainEvent;