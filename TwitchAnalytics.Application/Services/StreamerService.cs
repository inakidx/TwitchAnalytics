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
            Id = streamer.Id.ToString(),
            Username = streamer.Display_Name,
            Followers = streamer.Total_Followers.GetValueOrDefault(),
            Total_views = streamer.View_Count.GetValueOrDefault(),
            Created_At = streamer.Created_At,
            Last_Stream = streamer.Last_Stream_At
        };
    }
}
