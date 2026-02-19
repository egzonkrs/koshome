using System.Collections.Generic;
using System.Threading.Tasks;
using KosHome.Infrastructure.Authentication.Models;
using Refit;

namespace KosHome.Infrastructure.Authentication.Abstractions;

public interface IKeycloakAuthApi
{
    /// <summary>
    /// Provides a method to obtain an access token using the Resource Owner Password Credentials flow.
    /// </summary>
    [Post("/realms/{realm}/protocol/openid-connect/token")]
    [Headers("Content-Type: application/x-www-form-urlencoded")]
    Task<KeycloakTokenResponse> LoginAsync(string realm, [Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, string> loginRequest);
}