using TwitchAnalytics.Domain.Models;
using TwitchAnalytics.Domain.Repositories;

namespace TwitchAnalytics.Data.Repositories;

public class StreamerRepository : IStreamerRepository
{
    public Task<Streamer> Get(int id)
    {
        throw new NotImplementedException();
    }
}
