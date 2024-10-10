using Microsoft.Extensions.Configuration;
using TwitchAnalytics.Data.Core;
using TwitchAnalytics.Data.Core.AccessToken;
using TwitchAnalytics.Domain.Models;
using TwitchAnalytics.Domain.Repositories;

namespace TwitchAnalytics.Data.Repositories;

public class TwitchStreamRepository(IConfiguration configuration, AccessTokenService accessTokenService) : TwitchAPIRepository(configuration, accessTokenService), ITwitchStreamRepository
{
    public async Task<IEnumerable<TwitchStream>> GetStreamsAlive()
    {
        var apiParams = new KeyValuePair<string, string>[] {
            new("type", "live"),
            new("first", "100"),
        };
        IEnumerable<TwitchStream> streams = await GetResource<IEnumerable<TwitchStream>>("streams", apiParams);
        return streams;
    }
}
