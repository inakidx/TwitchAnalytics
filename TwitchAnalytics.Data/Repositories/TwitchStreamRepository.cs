using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http.Headers;
using TwitchAnalytics.Data.Core.AccessToken;
using TwitchAnalytics.Domain.Models;
using TwitchAnalytics.Domain.Repositories;

namespace TwitchAnalytics.Data.Repositories;

public class TwitchStreamRepository(IConfiguration configuration, AccessTokenService accessTokenService) : ITwitchStreamRepository
{
    private readonly IConfiguration _configuration = configuration;
    private readonly AccessTokenService _accessTokenService = accessTokenService;

    public async Task<IEnumerable<TwitchStream>> GetStreamsAlive()
    {
        var token = await _accessTokenService.GetAccessToken();

        HttpClient client = new();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Access_Token);
        client.DefaultRequestHeaders.Add("Client-ID", _configuration.GetSection("AuthSettings:ClientId").Value);

        var streamsAlive = await GetStreams(client);

        return streamsAlive;
    }

    private async Task<IEnumerable<TwitchStream>> GetStreams(HttpClient client)
    {
        string? uri = _configuration.GetConnectionString("TwitchAPI");
        var response = await client.GetAsync($"{uri}/streams?type=live&first=100");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync() ?? throw new KeyNotFoundException("error obtaining streams alive");

        JObject jsonObject = JObject.Parse(content);
        var dataNode = jsonObject["data"] ?? throw new Exception("streams parse error");

        var streamsAlive = JsonConvert.DeserializeObject<IEnumerable<TwitchStream>>(dataNode.ToString())
            ?? throw new Exception("streams parse error");
        return streamsAlive;
    }
}
