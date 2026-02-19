using FluentResults;
using MediatR;

namespace KosHome.Application.Users.Login;

public sealed record LoginUserCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;