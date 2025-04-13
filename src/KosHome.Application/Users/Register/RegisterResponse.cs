using System;

namespace KosHome.Application.Users.Register;

public record RegisterResponse
{
    public Ulid UserId { get; init; }
    public string IdentityId { get; init; }
}