using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net.Http.Headers;
using TwitchAnalytics.Data.Core.AccessToken;

namespace TwitchAnalytics.Data.Core;

public abstract class TwitchAPIRepository(IConfiguration configuration, AccessTokenService accessTokenService)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly AccessTokenService _accessTokenService = accessTokenService;

    public async Task<T> GetResource<T>(string resource, KeyValuePair<string, string>[] parameters)
    {
        string typeName = typeof(T).Name;
        var token = await _accessTokenService.GetAccessToken();

        HttpClient client = new();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Access_Token);
        client.DefaultRequestHeaders.Add("Client-ID", _configuration.GetSection("AuthSettings:ClientId").Value);

        string uri = _configuration.GetConnectionString("TwitchAPI") + "/" + resource;
        string query = string.Empty;
        if (parameters.Length != 0)
        {
            query = "?" + string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
        }
        var response = await client.GetAsync($"{uri}{query}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync() ?? throw new KeyNotFoundException($"{typeName} not found");
        JObject jsonObject = JObject.Parse(content);
        var dataNode = jsonObject["data"] ?? throw new Exception($"{typeName} parse error");

        return JsonConvert.DeserializeObject<T>(dataNode.ToString()) ?? throw new Exception($"{typeName} parse error");
    }
    public async Task<int> GetTotalResources(string resource, KeyValuePair<string, string>[] parameters)
    {
        var token = await _accessTokenService.GetAccessToken();

        HttpClient client = new();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Access_Token);
        client.DefaultRequestHeaders.Add("Client-ID", _configuration.GetSection("AuthSettings:ClientId").Value);

        string uri = _configuration.GetConnectionString("TwitchAPI") + "/" + resource;
        string query = string.Empty;
        if (parameters.Length != 0)
        {
            query = "?" + string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
        }
        var response = await client.GetAsync($"{uri}{query}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync() ?? throw new KeyNotFoundException($"total not found");
        JObject jsonObject = JObject.Parse(content);
        var dataNode = jsonObject["total"] ?? throw new Exception($"total parse error");
        return int.Parse(dataNode.ToString());
    }
}
