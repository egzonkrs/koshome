// using System;
// using System.Collections.Generic;
// using MediatR;
// using FluentResults;
// using System.Threading;
// using System.Threading.Tasks;
// using KosHome.Application.Abstractions.Auth.Services;
// using KosHome.Domain.Data.Abstractions;
// using KosHome.Domain.Data.Repositories;
// using KosHome.Domain.Entities.Users;
// using KosHome.Domain.ValueObjects.Users;
//
// namespace KosHome.Application.Users.Register;
//
// public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
// {
//     private readonly IIdentityService _identityService;
//     private readonly IUserRepository _userRepository;
//     private readonly IUnitOfWork _unitOfWork;
//
//     public RegisterUserCommandHandler(IIdentityService identityService, IUserRepository userRepository, IUnitOfWork unitOfWork)
//     {
//         _identityService = identityService;
//         _userRepository = userRepository;
//         _unitOfWork = unitOfWork;
//     }
//
//     public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
//     {
//         var user = User.Create(
//             new FirstName(request.IdentityUser.FirstName),
//             new LastName(request.IdentityUser.LastName),
//             new Email(request.IdentityUser.Email)
//         );
//         
//         var keycloakIdentityId = await _identityService.RegisterIdentityUserAsync(request.IdentityUser, cancellationToken);
//         user.SetIdentityId(keycloakIdentityId.ToString());
//
//         await _unitOfWork.ExecuteTransactionAsync(async x =>
//         {
//             await _userRepository.InsertAsync(user, cancellationToken);
//             await _unitOfWork.SaveChangesAsync(cancellationToken);
//             x.Complete();
//         });
//         
//         await _unitOfWork.SaveChangesAsync(cancellationToken);
//         throw new NotImplementedException();
//     }
// }