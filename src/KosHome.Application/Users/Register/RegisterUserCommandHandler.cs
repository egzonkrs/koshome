using System;
using MediatR;
using FluentResults;
using System.Threading;
using System.Threading.Tasks;

namespace KosHome.Application.Users.Register;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    public RegisterUserCommandHandler()
    {
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        return Result.Ok(Guid.NewGuid());
    }
}