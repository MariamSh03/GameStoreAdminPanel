using System.Net;
using System.Text;

namespace AdminPanel.Web.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var logMessage = new StringBuilder()
                .AppendLine($"=== Exception Details {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} ===")
                .AppendLine($"Exception Type: {exception.GetType().FullName}")
                .AppendLine($"Exception Message: {exception.Message}")
                .AppendLine($"Stack Trace: {exception.StackTrace}")
                .AppendLine("Inner Exception:");

            var innerException = exception.InnerException;
            while (innerException != null)
            {
                logMessage.AppendLine($"Type: {innerException.GetType().FullName}")
                         .AppendLine($"Message: {innerException.Message}")
                         .AppendLine($"Stack Trace: {innerException.StackTrace}")
                         .AppendLine();
                innerException = innerException.InnerException;
            }

            _logger.LogError(logMessage.ToString());

            await HandleExceptionAsync(context);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(new ErrorResponse
        {
            StatusCode = context.Response.StatusCode,
            Message = "An internal error occurred. Please try again later.",
        }.ToString());
    }
}