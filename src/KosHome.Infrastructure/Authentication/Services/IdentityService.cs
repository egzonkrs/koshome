using System.Threading.Tasks;
using FluentResults;
using Keycloak.Net;
using Keycloak.Net.Models.Users;
using KosHome.Application.Abstractions.Services;

namespace KosHome.Infrastructure.Authentication.Services;

public class IdentityService : IIdentityService
{
    private readonly KeycloakClient _keycloakClient;
    
    public IdentityService(KeycloakClient keycloakClient)
    {
        _keycloakClient = keycloakClient;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public Task<Result> RegisterUserAsync(string email, string password)
    {
        var qwe = _keycloakClient.CreateUserAsync("master", new User()
        {
            UserName = "asd"
        });
        
        throw new System.NotImplementedException();
    }
}