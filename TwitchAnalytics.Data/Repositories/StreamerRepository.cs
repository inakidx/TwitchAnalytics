using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using TwitchAnalytics.Data.Core.AccessToken;
using TwitchAnalytics.Domain.Models;
using TwitchAnalytics.Domain.Repositories;

namespace TwitchAnalytics.Data.Repositories;

public class StreamerRepository(IConfiguration configuration, AccessTokenService accessTokenService) : IStreamerRepository
{
    private readonly IConfiguration _configuration = configuration;
    private readonly AccessTokenService _accessTokenService = accessTokenService;

    public async Task<Streamer> Get(int id)
    {
        var token = await _accessTokenService.GetAccessToken();

        HttpClient client = new();
        string? uri = _configuration.GetConnectionString("TwitchAPI");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Access_Token);
        client.DefaultRequestHeaders.Add("Client-ID", _configuration.GetSection("AuthSettings:ClientId").Value);

        var response = await client.GetAsync($"{uri}/users?id={id}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync() ?? throw new KeyNotFoundException("user not found");

        JObject jsonObject = JObject.Parse(content);
        var dataNode = jsonObject["data"] ?? throw new Exception("user parse error");
        Streamer streamer = JsonConvert.DeserializeObject<ICollection<Streamer>>(dataNode.ToString())?.FirstOrDefault()
            ?? throw new Exception("user parse error");

        return streamer;
    }
}
