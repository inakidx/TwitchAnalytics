using Microsoft.AspNetCore.Http;
using System.Net;

namespace TwitchAnalytics.Exceptions;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

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

    private static Task HandleException(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case ArgumentNullException:
                code = HttpStatusCode.BadRequest;
                result = "The request is missing required information.";
                break;
            case InvalidOperationException:
                code = HttpStatusCode.BadRequest;
                result = "The request is invalid.";
                break;
            case KeyNotFoundException:
                code = HttpStatusCode.NotFound;
                result = "The requested resource was not found.";
                break;
            case UnauthorizedAccessException:
                code = HttpStatusCode.Unauthorized;
                result = "You are not authorized to access this resource.";
                break;
            case Exception:
                code = HttpStatusCode.InternalServerError;
                result = "An unexpected error occurred.";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}
