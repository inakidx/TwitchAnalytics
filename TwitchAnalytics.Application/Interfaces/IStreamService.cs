using TwitchAnalytics.Application.DTO;

namespace TwitchAnalytics.Application.Interfaces;

public interface IStreamService
{
    Task<ICollection<TwitchStreamDTO>> GetStreamsAlive();
}
