using TwitchAnalytics.Application.DTO;

namespace TwitchAnalytics.Application.Interfaces;

public interface ITwitchStreamService
{
    Task<ICollection<TwitchStreamDTO>> GetStreamsAlive();
}
