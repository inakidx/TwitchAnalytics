using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Authentication;
using TwitchAnalytics.Intrastructure.Configuration;

namespace TwitchAnalytics.Application.Core.AccessToken;

public static class AccessTokenUtils
{
    private const string TOKEN_CACHE_KEY = "OAuthToken";
    public static async Task<TokenResponse> GetAccessToken(IMemoryCache cache, IOptions<TwitchAPIConfiguration> configuration)
    {
        if (!cache.TryGetValue(TOKEN_CACHE_KEY, out TokenResponse? accessToken))
        {
            accessToken = await GenerateAccessToken(configuration);
            if (accessToken != null && !accessToken.IsError)
            {
                cache.Set(TOKEN_CACHE_KEY, accessToken, TimeSpan.FromSeconds(accessToken.ExpiresIn));
            }
        }
        if (accessToken == null || accessToken.IsError)
        {
            throw new AuthenticationException("Failed to generate access token");
        }
        return accessToken;
    }

    private static async Task<TokenResponse?> GenerateAccessToken(IOptions<TwitchAPIConfiguration> configuration)
    {
        HttpClient client = new();

        string clientId = configuration.Value.ClientId;
        string clientSecret = configuration.Value.ClientSecret;
        string authenticationUri = configuration.Value.RedirectUri;

        var body = new Dictionary<string, string>
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
        return JsonConvert.DeserializeObject<TokenResponse>(content);
    }
}
