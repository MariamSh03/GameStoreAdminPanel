using System.Text;
using AdminPanel.Web.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminPanel.Tests.Midlware.Tests;

public class RequestLoggingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_LogsRequestAndResponseDetails()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<RequestLoggingMiddleware>>();
        var nextMock = new Mock<RequestDelegate>();
        nextMock
            .Setup(next => next(It.IsAny<HttpContext>()))
            .Returns(async (HttpContext context) =>
            {
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("Response content");
            });

        var middleware = new RequestLoggingMiddleware(nextMock.Object, loggerMock.Object);
        var context = CreateHttpContext("GET", "/test", "Request content");

        // Act
        await middleware.InvokeAsync(context);

        // Assert
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        loggerMock.Verify(
            static logger =>
            logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Request Details")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

        // Verify that the response body is correct after the middleware processes it
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        Assert.Equal("Response content", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_HandlesEmptyRequestBodyGracefully()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<RequestLoggingMiddleware>>();
        var nextMock = new Mock<RequestDelegate>();
        nextMock
            .Setup(next => next(It.IsAny<HttpContext>()))
            .Returns(async (HttpContext context) =>
            {
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync("Response content");
            });

        var middleware = new RequestLoggingMiddleware(nextMock.Object, loggerMock.Object);
        var context = CreateHttpContext("POST", "/empty", string.Empty);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        loggerMock.Verify(
            logger =>
            logger.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Request Details")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
    }

    private static HttpContext CreateHttpContext(string method, string path, string requestBody)
    {
        var context = new DefaultHttpContext();

        // Set request method, path, and body
        context.Request.Method = method;
        context.Request.Path = path;
        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
        context.Request.ContentLength = requestBody.Length;

        // Set response body
        context.Response.Body = new MemoryStream();

        // Mock remote IP for logging
        context.Connection.RemoteIpAddress = System.Net.IPAddress.Parse("127.0.0.1");

        return context;
    }
}
