using System;
using KosHome.Domain.Abstractions;

namespace KosHome.Domain.Events.ApartmentImages;

public sealed record ApartmentImageAddedDomainEvent(Ulid ApartmentImageId) : IDomainEvent;