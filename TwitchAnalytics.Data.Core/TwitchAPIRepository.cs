using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace TwitchAnalytics.Data.Core;

public abstract class TwitchAPIRepository(IConfiguration configuration, ApiClient apiClient)
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ApiClient _apiClient = apiClient;

    public async Task<T> GetResource<T>(string resource, KeyValuePair<string, string>[] parameters)
    {
        HttpClient client = await _apiClient.GetAuthenticatedClient();
        var response = await client.GetAsync(GetUrl(resource, parameters));
        response.EnsureSuccessStatusCode();

        return await GetParsedResource<T>(response);
    }

    public async Task<int> GetTotalResources(string resource, KeyValuePair<string, string>[] parameters)
    {
        HttpClient client = await _apiClient.GetAuthenticatedClient();

        string url = GetUrl(resource, parameters);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync() ?? throw new KeyNotFoundException($"total not found");
        JObject jsonObject = JObject.Parse(content);
        var dataNode = jsonObject["total"] ?? throw new Exception($"total parse error");
        return int.Parse(dataNode.ToString());
    }

    private string GetUrl(string resource, KeyValuePair<string, string>[] parameters)
    {
        string uri = _configuration.GetConnectionString("TwitchAPI") + "/" + resource;

        string query = string.Empty;
        if (parameters.Length != 0)
        {
            query = "?" + string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"));
        }
        return uri + query;
    }

    private static async Task<T> GetParsedResource<T>(HttpResponseMessage response)
    {
        string typeName = typeof(T).Name;
        var content = await response.Content.ReadAsStringAsync() ?? throw new KeyNotFoundException($"{typeName} not found");
        JObject jsonObject = JObject.Parse(content);
        var dataNode = jsonObject["data"] ?? throw new Exception($"{typeName} parse error");

        return JsonConvert.DeserializeObject<T>(dataNode.ToString()) ?? throw new Exception($"{typeName} parse error");
    }
}
