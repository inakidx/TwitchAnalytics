using TwitchAnalytics.Application.DTO;
using TwitchAnalytics.Application.Interfaces;
using TwitchAnalytics.Domain.Repositories;

namespace TwitchAnalytics.Application.Services;

public class StreamService(IStreamRepository streamRepository) : IStreamService
{
    private readonly IStreamRepository _streamRepository = streamRepository;

    public async Task<ICollection<TwitchStreamDTO>> GetStreamsAlive()
    {
        var streamsAlive = await _streamRepository.GetStreamsAlive();

        return streamsAlive.Select(stream => new TwitchStreamDTO()
        {
            stream_id = stream.Id,
            username = stream.UserName,
            title = stream.Title,
            viewer_count = stream.ViewerCount,
            started_at = stream.StartedAt
        }).ToList();
    }
}
