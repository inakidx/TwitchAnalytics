using TwitchAnalytics.Domain.Models;

namespace TwitchAnalytics.Domain.Repositories;

public interface ITwitchStreamRepository
{
    Task<IEnumerable<TwitchStream>> GetStreamsAlive();
}
