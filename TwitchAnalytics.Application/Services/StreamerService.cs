using TwitchAnalytics.Application.DTO;
using TwitchAnalytics.Application.Interfaces;
using TwitchAnalytics.Domain.Models;
using TwitchAnalytics.Domain.Repositories;

namespace TwitchAnalytics.Application.Services;

public class StreamerService(IStreamerRepository streamerRepository) : IStreamerService
{
    private readonly IStreamerRepository _streamerRepository = streamerRepository;

    public async Task<StreamerDTO> Get(int id)
    {
        Streamer streamer = await _streamerRepository.Get(id);
        return new StreamerDTO()
        {
            id = streamer.Id,
            username = streamer.UserName,
            followers = streamer.TotalFollowers,
            total_views = streamer.TotalViews,
            created_at = streamer.CreatedAt,
            last_stream = streamer.LastStreamAt
        };
    }
}
