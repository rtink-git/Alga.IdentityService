namespace Alga.IdentityService.Infrastructure.HTTP.Endpoint;

internal static class ErrorHandler
{
    public static async ValueTask HandleExceptionAsync(HttpContext context, ILogger logger, Exception ex)
    {
        var statusCode = ex switch
        {
            HttpRequestException => 503,
            _ => 500
        };

        var message = string.IsNullOrWhiteSpace(ex.Message) ? string.Empty : $" | {ex.Message}";

        logger.LogError($"{context.Connection.RemoteIpAddress} | {context.Request.Path} | {statusCode} {message}");

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(statusCode.ToString());
    }
}