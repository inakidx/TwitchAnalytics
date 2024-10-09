using System.Net;

namespace TwitchAnalytics.Exceptions;

public class ExceptionDTO
{
    public HttpStatusCode Code { get; set; }
    public string Message { get; set; } = string.Empty;
}
