using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using Keycloak.Net;
using KosHome.Domain.Common;
using Microsoft.Extensions.Logging;
using KosHome.Domain.Entities.Users;
using KosHome.Application.Abstractions.Auth.Services;
using KeycloakUser = Keycloak.Net.Models.Users.User;

namespace KosHome.Infrastructure.Authentication.Services;

public sealed class IdentityService : IIdentityService
{
    private readonly KeycloakClient _keycloakClient; // Maybe switch to factory pattern in the future since we don't have interface here
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(KeycloakClient keycloakClient, ILogger<IdentityService> logger)
    {
        _keycloakClient = keycloakClient;
        _logger = logger;
    }
    
    public async Task<Result<Guid>> RegisterIdentityUserAsync(IdentityUser identityUser, CancellationToken cancellationToken = default)
    {
        try
        {
            var keycloakIdentityUser = new KeycloakUser
            {
                FirstName = identityUser.FirstName,
                LastName = identityUser.LastName,
                Email = identityUser.Email,
                Enabled = identityUser.IsEnabled ?? true,
                EmailVerified = identityUser.IsEmailVerified ?? false,
                Credentials = new List<Keycloak.Net.Models.Users.Credentials>()
                {
                    new()
                    {
                        Type = "password",
                        Value = identityUser.Password,
                        Temporary = false
                    }
                },
                Attributes = identityUser.Attributes?.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.AsEnumerable() ?? []
                ),
            };

            var identityUserId = await _keycloakClient.CreateAndRetrieveUserIdAsync(realm: "koshome", keycloakIdentityUser);
            var allRealmRoles = await _keycloakClient.GetRolesAsync("koshome");

            var role = allRealmRoles.FirstOrDefault(x => x.Name is "user");

            if (role is null)
            {
                _logger.LogInformation("Failed to find the role 'user' in the realm 'koshome'");
                return Result.Fail(UsersErrors.NotFound("user"));
            }

            var addingRolesToUserIsSuccess = await _keycloakClient.AddRealmRoleMappingsToUserAsync(
                "createIdentityUser.RealmId", 
                identityUserId,
                new[] { role });

            if (addingRolesToUserIsSuccess is false)
            {
                _logger.LogInformation("Failed to add roles to the identity user with Id: {IdentityUserId}", identityUserId);
                return Result.Fail(UsersErrors.IdentityUserInvalid);
            }
            
            _logger.LogInformation("Successfully created an identity user with Id: {IdentityUserId}", identityUserId);
            return Result.Ok(Guid.Parse(identityUserId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create an Identity user with Email: {Email} with Exception message: {Message}", identityUser.Email, 
                ex.Message);

            return Result.Fail(UsersErrors.UnexpectedError());
        }
    }
}