namespace TwitchAnalytics.Application.DTO;

public class StreamerDTO
{
    public string Id { get; set; } = null!;
    public string? Username { get; set; }
    public int Followers { get; set; }
    public int Total_views { get; set; }
    public DateTime? Created_At { get; set; }
    public DateTime? Last_Stream { get; set; }
}
