using TwitchAnalytics.Application.DTO;

namespace TwitchAnalytics.Application.Interfaces
{
    public interface IStreamerService
    {
        public Task<StreamerDTO> Get(int id);
    }
}
