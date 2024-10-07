namespace TwitchAnalytics.Intrastructure.Configuration;

public class TwitchAPIAuthenticationConfiguration
{
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string RedirectUri { get; set; } = null!;
}
