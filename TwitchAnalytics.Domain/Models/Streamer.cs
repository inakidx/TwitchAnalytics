namespace TwitchAnalytics.Domain.Models;

public class Streamer
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public int TotalFollowers { get; set; }
    public int TotalViews { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastStreamAt { get; set; }

    private Streamer()
    {
        Id = 0;
        UserName = string.Empty;
        TotalFollowers = 0;
        TotalViews = 0;
        CreatedAt = DateTime.MinValue;
        LastStreamAt = DateTime.MinValue;
    }
    public static Streamer Create(int id, string userName, int totalFollowers, int totalViews, DateTime createdAt, DateTime lastStreamAt)
    {
        return new Streamer()
        {
            Id = id,
            UserName = userName,
            TotalFollowers = totalFollowers,
            TotalViews = totalViews,
            CreatedAt = createdAt,
            LastStreamAt = lastStreamAt
        };
    }
}
