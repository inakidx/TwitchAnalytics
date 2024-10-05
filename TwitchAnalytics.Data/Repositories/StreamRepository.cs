using TwitchAnalytics.Domain.Models;
using TwitchAnalytics.Domain.Repositories;

namespace TwitchAnalytics.Data.Repositories;

public class StreamRepository : IStreamRepository
{
    public Task<IEnumerable<TwitchStream>> GetStreamsAlive()
    {
        throw new NotImplementedException();
    }
}
