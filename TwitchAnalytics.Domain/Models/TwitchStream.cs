namespace TwitchAnalytics.Domain.Models;

public class TwitchStream
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Title { get; set; }
    public int ViewerCount { get; set; }
    public DateTime StartedAt { get; set; }

    private TwitchStream()
    {
        Id = 0;
        UserId = 0;
        UserName = string.Empty;
        Title = string.Empty;
        ViewerCount = 0;
        StartedAt = DateTime.MinValue;
    }

    public static TwitchStream Create(int streamId, int userId, string userName, string title, int viewerCount, DateTime startedAt)
    {
        return new TwitchStream
        {
            Id = streamId,
            UserId = userId,
            UserName = userName,
            Title = title,
            ViewerCount = viewerCount,
            StartedAt = startedAt
        };
    }
}
