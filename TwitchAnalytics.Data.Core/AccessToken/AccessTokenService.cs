using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Security.Authentication;

namespace TwitchAnalytics.Data.Core.AccessToken;

public class AccessTokenService(IMemoryCache cache, IConfiguration configuration)
{
    private const string TOKEN_CACHE = "OAuthToken";
    private const string CLIENT_ID_SECTION = "AuthSettings:ClientId";
    private const string CLIENT_SECRET_SECTION = "AuthSettings:ClientSecret";
    private const string AUTHENTICATION_URI_SECTION = "AuthSettings:AuthAPIUri";

    private readonly IMemoryCache _cache = cache;
    private readonly IConfiguration _configuration = configuration;

    public async Task<AccessTokenDTO> GetAccessToken()
    {
        if (!_cache.TryGetValue(TOKEN_CACHE, out AccessTokenDTO? accessToken))
        {
            accessToken = await GenerateAccessToken();
            if (accessToken != null)
            {
                _cache.Set(TOKEN_CACHE, accessToken, TimeSpan.FromMilliseconds(accessToken.Expires_In));
            }
        }
        if (accessToken == null)
        {
            throw new AuthenticationException("Failed to generate access token");
        }
        return accessToken;
    }

    private async Task<AccessTokenDTO?> GenerateAccessToken()
    {
        HttpClient client = new();

        string? clientId = _configuration.GetRequiredSection(CLIENT_ID_SECTION).Value;
        string? clientSecret = _configuration.GetRequiredSection(CLIENT_SECRET_SECTION).Value;
        string? authenticationUri = _configuration.GetRequiredSection(AUTHENTICATION_URI_SECTION).Value;

        var body = new Dictionary<string, string?>
        {
            { "grant_type", "client_credentials" },
            { "client_id", clientId },
            { "client_secret", clientSecret }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, authenticationUri)
        {
            Content = new FormUrlEncodedContent(body)
        };

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return AccessTokenDTO.Parse(content);
    }
}
