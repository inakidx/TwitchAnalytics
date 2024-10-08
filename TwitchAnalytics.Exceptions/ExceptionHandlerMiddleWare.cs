using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace TwitchAnalytics.Exceptions;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private Task HandleException(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case ArgumentNullException:
                code = HttpStatusCode.BadRequest;
                result = "Missing 'Id' paramether.";
                break;
            case InvalidOperationException:
                code = HttpStatusCode.BadRequest;
                result = "Invalid 'Id' paramether.";
                break;
            case KeyNotFoundException:
                code = HttpStatusCode.NotFound;
                result = "User not found.";
                break;
            case UnauthorizedAccessException:
                code = HttpStatusCode.Unauthorized;
                result = "Unauthorized. Twitch access token is invalid or has expired.";
                break;
            case Exception:
                code = HttpStatusCode.InternalServerError;
                result = "An unexpected error occurred.";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        _logger.LogError($"Exception: {exception.ToString()}. Returned-> code: {code}, value: {result}");

        return context.Response.WriteAsync(result);
    }
}
