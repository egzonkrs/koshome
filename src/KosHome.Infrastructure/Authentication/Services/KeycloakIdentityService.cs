using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Domain.Common;
using Microsoft.Extensions.Logging;
using KosHome.Domain.Entities.Users;
using KosHome.Application.Abstractions.Auth.Services;
using KosHome.Infrastructure.Authentication.Abstractions;
using KosHome.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using KeycloakUser = Keycloak.Net.Models.Users.User;

namespace KosHome.Infrastructure.Authentication.Services;

public sealed class KeycloakIdentityService : IKeycloakIdentityService
{
    private readonly IKeycloakClientWrapper _keycloakWrapper;
    private readonly ILogger<KeycloakIdentityService> _logger;
    private readonly IOptions<AuthenticationOptions> _authOptions;
    private readonly IKeycloakAuthApi _keycloakAuthApi;
    private readonly string _realmName;

    public KeycloakIdentityService(
        IKeycloakClientWrapper keycloakWrapper,
        ILogger<KeycloakIdentityService> logger,
        IOptions<AuthenticationOptions> authOptions, 
        IKeycloakAuthApi keycloakAuthApi)
    {
        _logger = logger;
        _authOptions = authOptions;
        _keycloakWrapper = keycloakWrapper;
        _keycloakAuthApi = keycloakAuthApi;
        _realmName = authOptions.Value.Keycloak.Realm;
    }
    
    public async Task<Result<Guid>> CreateIdentityUserAndAssignRoleAsync(IdentityUser identityUser, CancellationToken cancellationToken = default)
    {
        try
        {
            if (identityUser is null)
            {
                return Result.Fail(UsersErrors.IdentityUserInvalid);
            }
            
            var keycloakUser = MapToKeycloakUser(identityUser);

            var createUserResult = await _keycloakWrapper.CreateAndRetrieveUserIdAsync(_realmName, keycloakUser, cancellationToken);
            if (createUserResult.IsFailed)
            {
                _logger.LogError("Failed to create identity user with Email: {Email}", identityUser.Email);
                return Result.Fail(UsersErrors.IdentityUserInvalid);
            }

            // var allRealmRoles = await _keycloakWrapper.GetRolesAsync(_realmName, cancellationToken: cancellationToken);
            // var role = allRealmRoles.FirstOrDefault(x => x.Name is "user");
            //
            // if (role is null)
            // {
            //     _logger.LogInformation("Failed to find the role 'user' in the realm '{Realm}'", _realmName);
            //     return Result.Fail(UsersErrors.NotFound("user"));
            // }
            //
            var identityUserId = createUserResult.Value;
            // var addingRolesToUserIsSuccess = await _keycloakWrapper.AddRealmRoleMappingsToUserAsync(
            //     _realmName, 
            //     identityUserId,
            //     [new Role { Name = "user" }], cancellationToken);
            //
            // if (addingRolesToUserIsSuccess.IsFailed)
            // {
            //     _logger.LogInformation("Failed to add roles to the identity user with Id: {IdentityUserId}", identityUserId);
            //     return Result.Fail(UsersErrors.IdentityUserInvalid);
            // }
            
            _logger.LogInformation("Successfully created an identity user with Id: {IdentityUserId}", identityUserId);
            return Result.Ok(Guid.Parse(identityUserId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create identity user with email {Email}: {Message}", identityUser?.Email, ex.Message);
            return Result.Fail(UsersErrors.UnexpectedError());
        }
    }
    
    public async Task<Result<string>> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var parameters = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", _authOptions.Value.Keycloak.ClientId },
                { "client_secret", _authOptions.Value.Keycloak.ClientSecret },
                { "username", username },
                { "password", password }
            };

            var tokenResponse = await _keycloakAuthApi.LoginAsync(_realmName, parameters);
            return Result.Ok(tokenResponse.AccessToken);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Failed to login user {username} with Exception Message: {ExMessage}", username, ex.Message);
            return Result.Fail(UsersErrors.UnexpectedError());
        }
    }
    
    private static KeycloakUser MapToKeycloakUser(IdentityUser identityUser)
    {
        return new KeycloakUser
        {
            FirstName = identityUser.FirstName,
            LastName = identityUser.LastName,
            UserName = identityUser.Email,
            Email = identityUser.Email,
            Enabled = identityUser.IsEnabled,
            EmailVerified = identityUser.IsEmailVerified,
            Credentials = new List<Keycloak.Net.Models.Users.Credentials>
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
                kvp => kvp.Value?.AsEnumerable() ?? []) ?? new Dictionary<string, IEnumerable<string>>()
        };
    }
}