using System;
using KosHome.Domain.Abstractions;

namespace KosHome.Domain.Events.Users;

public sealed record UserCreatedDomainEvent(Ulid UserId) : IDomainEvent;