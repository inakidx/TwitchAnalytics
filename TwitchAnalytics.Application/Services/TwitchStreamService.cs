using TwitchAnalytics.Application.DTO;
using TwitchAnalytics.Application.Interfaces;
using TwitchAnalytics.Domain.Repositories;

namespace TwitchAnalytics.Application.Services;

public class TwitchStreamService(ITwitchStreamRepository streamRepository) : ITwitchStreamService
{
    private readonly ITwitchStreamRepository _streamRepository = streamRepository;

    public async Task<ICollection<TwitchStreamDTO>> GetStreamsAlive()
    {
        var streamsAlive = await _streamRepository.GetStreamsAlive();

        return streamsAlive.Select(stream => new TwitchStreamDTO()
        {
            Stream_Id = stream.Id.ToString(),
            Username = stream.User_Name,
            Title = stream.Title,
            Viewer_Count = stream.Viewer_Count,
            Started_At = stream.Started_At
        }).ToList();
    }
}
