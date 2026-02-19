using System;
using System.Collections.Generic;

namespace KosHome.Application.Users.GetCurrentUser;

public sealed record CurrentUserResponse
{
    public Ulid UserId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public IEnumerable<string> Roles { get; init; } = [];
}
