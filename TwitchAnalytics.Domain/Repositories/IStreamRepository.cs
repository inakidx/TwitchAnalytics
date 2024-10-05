using TwitchAnalytics.Domain.Models;

namespace TwitchAnalytics.Domain.Repositories;

public interface IStreamRepository
{
    Task<IEnumerable<TwitchStream>> GetStreamsAlive();
}
