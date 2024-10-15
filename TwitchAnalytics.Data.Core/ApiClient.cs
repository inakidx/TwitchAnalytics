using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using TwitchAnalytics.Data.Core.AccessToken;

namespace TwitchAnalytics.Data.Core;

public class ApiClient(AccessTokenService accessTokenService, IConfiguration configuration)
{
    private readonly AccessTokenService _accessTokenService = accessTokenService;
    private readonly IConfiguration _configuration = configuration;

    public async Task<HttpClient> GetAuthenticatedClient()
    {
        var token = await _accessTokenService.GetAccessToken();

        HttpClient client = new();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Access_Token);
        client.DefaultRequestHeaders.Add("Client-ID", _configuration.GetSection("AuthSettings:ClientId").Value);
        return client;
    }
}
