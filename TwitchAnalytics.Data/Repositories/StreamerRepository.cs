using Microsoft.Extensions.Configuration;
using TwitchAnalytics.Data.Core;
using TwitchAnalytics.Data.Core.AccessToken;
using TwitchAnalytics.Domain.Models;
using TwitchAnalytics.Domain.Repositories;

namespace TwitchAnalytics.Data.Repositories;

public class StreamerRepository(IConfiguration configuration, AccessTokenService accessTokenService) : TwitchAPIRepository(configuration, accessTokenService), IStreamerRepository
{
    public async Task<Streamer> Get(string id)
    {
        var apiParams = new KeyValuePair<string, string>[] {
            new("id", id)
        };
        var streamers = await GetResource<Streamer[]>("users", apiParams);
        if (streamers == null || streamers.Length == 0)
        {
            throw new KeyNotFoundException("User not found");
        }
        Streamer streamer = streamers[0];

        apiParams = [
            new("broadcaster_id", id),
            new("first", "100"),
        ];
        int totalFollowers = await GetTotalResources("channels/followers", apiParams);

        streamer.Total_Followers = totalFollowers;

        apiParams = [
            new("user_id", id),
            new("first", "100"),
        ];
        var streams = await GetResource<IEnumerable<TwitchStream>>("streams", apiParams);
        streamer.Last_Stream_At = streams.OrderByDescending(s => s.Started_At).FirstOrDefault()?.Started_At;

        return streamer;
    }
}