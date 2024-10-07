using Newtonsoft.Json;

namespace TwitchAnalytics.Data.Core.AccessToken;

public class AccessTokenDTO
{
    public string Access_Token { get; set; } = null!;
    public string? Token_Type { get; set; }
    public int Expires_In { get; set; }

    public static AccessTokenDTO Parse(string json)
    {
        return JsonConvert.DeserializeObject<AccessTokenDTO>(json) ?? throw new InvalidOperationException("bad json format");
    }
}
