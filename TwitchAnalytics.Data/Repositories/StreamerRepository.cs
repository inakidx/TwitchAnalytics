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

    public async Task<Streamer> Get(string id)
    {
        var token = await _accessTokenService.GetAccessToken();

        HttpClient client = new();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Access_Token);
        client.DefaultRequestHeaders.Add("Client-ID", _configuration.GetSection("AuthSettings:ClientId").Value);

        var streamer = await GetStreamer(client, id);

        return streamer;
    }

    public async Task<Streamer> GetStreamer(HttpClient client, string id)
    {
        string? uri = _configuration.GetConnectionString("TwitchAPI");
        var response = await client.GetAsync($"{uri}/users?id={id}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync() ?? throw new KeyNotFoundException("user not found");

        JObject jsonObject = JObject.Parse(content);
        var dataNode = jsonObject["data"] ?? throw new Exception("user parse error");
        if (!dataNode.Any())
        {
            throw new KeyNotFoundException("user not found");
        }
        Streamer streamer = JsonConvert.DeserializeObject<ICollection<Streamer>>(dataNode.ToString())?.FirstOrDefault()
            ?? throw new Exception("user parse error");

        response = await client.GetAsync($"{uri}/channels/followers?broadcaster_id={id}");
        response.EnsureSuccessStatusCode();
        content = await response.Content.ReadAsStringAsync() ?? throw new KeyNotFoundException("user not found");
        jsonObject = JObject.Parse(content);
        int totalFollowers = int.Parse(jsonObject["total"]?.ToString() ?? throw new Exception("followers parse error"));
        streamer.Total_Followers = totalFollowers;

        response = await client.GetAsync($"{uri}/streams?user_id={id}&first=100");
        response.EnsureSuccessStatusCode();
        content = await response.Content.ReadAsStringAsync() ?? throw new KeyNotFoundException("streams not found");
        jsonObject = JObject.Parse(content);
        dataNode = jsonObject["data"] ?? throw new Exception("streams parse error");
        var streams = JsonConvert.DeserializeObject<IEnumerable<TwitchStream>>(dataNode.ToString())
                    ?? throw new Exception("streams parse error");
        streamer.Last_Stream_At = streams.OrderByDescending(s => s.Started_At).FirstOrDefault()?.Started_At;

        return streamer;
    }
}
