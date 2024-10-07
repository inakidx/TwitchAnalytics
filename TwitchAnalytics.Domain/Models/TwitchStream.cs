using System.Numerics;

namespace TwitchAnalytics.Domain.Models;

public class TwitchStream
{
    public BigInteger Id { get; set; }
    public int User_Id { get; set; }
    public string? User_Name { get; set; }
    public string? Title { get; set; }
    public int Viewer_Count { get; set; }
    public DateTime Started_At { get; set; }
}
