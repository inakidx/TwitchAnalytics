namespace TwitchAnalytics.Application.DTO;

public class StreamerDTO
{
    public int id;
    public string? username;
    public int followers;
    public int total_views;
    public DateTime? created_at;
    public DateTime? last_stream;
}
