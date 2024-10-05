using TwitchAnalytics.Domain.Models;

namespace TwitchAnalytics.Domain.Repositories;

public interface IStreamerRepository
{
    public Task<Streamer> Get(int id);
}
