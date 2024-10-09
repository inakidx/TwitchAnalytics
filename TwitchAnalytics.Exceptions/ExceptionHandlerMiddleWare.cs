using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;

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
        var exceptionDTO = new ExceptionDTO()
        {
            Code = HttpStatusCode.InternalServerError,
            Message = string.Empty
        };

        switch (exception)
        {
            case ArgumentNullException:
                exceptionDTO.Code = HttpStatusCode.BadRequest;
                exceptionDTO.Message = "Missing 'Id' paramether.";
                break;
            case InvalidOperationException:
                exceptionDTO.Code = HttpStatusCode.BadRequest;
                exceptionDTO.Message = "Invalid 'Id' paramether.";
                break;
            case KeyNotFoundException:
                exceptionDTO.Code = HttpStatusCode.NotFound;
                exceptionDTO.Message = "User not found.";
                break;
            case UnauthorizedAccessException:
                exceptionDTO.Code = HttpStatusCode.Unauthorized;
                exceptionDTO.Message = "Unauthorized. Twitch access token is invalid or has expired.";
                break;
            case Exception:
                exceptionDTO.Code = HttpStatusCode.InternalServerError;
                exceptionDTO.Message = "An unexpected error occurred.";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)exceptionDTO.Code;

        _logger.LogError($"Exception: {exception.ToString()}. Returned-> code: {exceptionDTO.Code}, value: {exceptionDTO.Message}");

        return context.Response.WriteAsync(JsonConvert.SerializeObject(exceptionDTO));
    }
}
