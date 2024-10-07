using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        string? uri = _configuration.GetConnectionString("TwitchAPI");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Access_Token);
        client.DefaultRequestHeaders.Add("Client-ID", _configuration.GetSection("AuthSettings:ClientId").Value);

        var response = await client.GetAsync($"{uri}/streams?type=live");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync() ?? throw new KeyNotFoundException("error obtaining streams alive");

        JObject jsonObject = JObject.Parse(content);
        var dataNode = jsonObject["data"] ?? throw new Exception("streams parse error");

        var streamsAlive = JsonConvert.DeserializeObject<IEnumerable<TwitchStream>>(dataNode.ToString())
            ?? throw new Exception("streams parse error");

        return streamsAlive;
    }
}
