using System.Numerics;

namespace TwitchAnalytics.Domain.Models;

public class Streamer
{
    public BigInteger Id { get; set; }
    public string? Display_Name { get; set; }
    public int Total_Followers { get; set; }
    public int View_Count { get; set; }
    public DateTime? Created_At { get; set; }
    public DateTime? Last_Stream_At { get; set; }
}
