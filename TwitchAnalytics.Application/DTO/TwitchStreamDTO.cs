namespace TwitchAnalytics.Application.DTO;

public class TwitchStreamDTO
{
    public string Stream_Id { get; set; } = null!;
    public string? Username { get; set; }
    public string? Title { get; set; }
    public int Viewer_Count { get; set; }
    public DateTime? Started_At { get; set; }
}
