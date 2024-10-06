using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TwitchAnalytics.Data.Core.AccessToken;
using TwitchAnalytics.Domain.Models;
using TwitchAnalytics.Domain.Repositories;
using TwitchAnalytics.Intrastructure.Configuration;

namespace TwitchAnalytics.Data.Repositories;

public class StreamerRepository(IMemoryCache cache, IOptions<TwitchAPIConfiguration> twitchAPIConfiguration) : IStreamerRepository
{
    private readonly IMemoryCache _cache = cache;
    private readonly IOptions<TwitchAPIConfiguration> _twitchAPIConfiguration = twitchAPIConfiguration;

    public async Task<Streamer> Get(int id)
    {
        var token = await AccessTokenUtils.GetAccessToken(_cache, _twitchAPIConfiguration);
        throw new NotImplementedException();
    }
}
