using System.Diagnostics;
using System.Text;

namespace AdminPanel.Web.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestBody = await GetRequestBody(context.Request);
        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);

            stopwatch.Stop();
            var responseContent = await GetResponseBody(context.Response);

            var logMessage = new StringBuilder()
                .AppendLine($"=== Request Details {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} ===")
                .AppendLine($"IP Address: {context.Connection.RemoteIpAddress}")
                .AppendLine($"URL: {context.Request.Method} {context.Request.Path}{context.Request.QueryString}")
                .AppendLine($"Status Code: {context.Response.StatusCode}")
                .AppendLine($"Elapsed Time: {stopwatch.ElapsedMilliseconds}ms")
                .AppendLine("Request Content:")
                .AppendLine(requestBody)
                .AppendLine("Response Content:")
                .AppendLine(responseContent)
                .ToString();

            _logger.LogInformation(logMessage);
        }
        finally
        {
            await CopyResponseBody(responseBody, originalBodyStream);
            context.Response.Body = originalBodyStream;
        }
    }

    private static async Task<string> GetRequestBody(HttpRequest request)
    {
        request.EnableBuffering();
        using var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true);
        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;
        return body;
    }

    private static async Task<string> GetResponseBody(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        var text = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin);
        return text;
    }

    private static async Task CopyResponseBody(Stream sourceStream, Stream destinationStream)
    {
        sourceStream.Position = 0;
        await sourceStream.CopyToAsync(destinationStream);
    }
}