using System.Net;
using AdminPanel.Web.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace AdminPanel.Tests.Midlware.Tests;

public class GlobalExceptionHandlerMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_LogsExceptionDetailsAndSetsErrorResponse()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GlobalExceptionHandlerMiddleware>>();
        var nextMock = new Mock<RequestDelegate>();
        nextMock
            .Setup(next => next(It.IsAny<HttpContext>()))
            .Throws(new Exception("Test exception"));

        var middleware = new GlobalExceptionHandlerMiddleware(nextMock.Object, loggerMock.Object);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        // Verify that the response is set to 500 Internal Server Error
        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);

        // Verify that the response body contains the correct error message
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        Assert.Contains("An internal error occurred. Please try again later.", responseBody);

        // Verify the exception was logged
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        loggerMock.Verify(
            logger =>
            logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Exception Details")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
    }

    [Fact]
    public async Task InvokeAsync_HandlesInnerExceptionAndLogsDetails()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GlobalExceptionHandlerMiddleware>>();
        var nextMock = new Mock<RequestDelegate>();
        nextMock
            .Setup(next => next(It.IsAny<HttpContext>()))
            .Throws(new Exception("Outer exception", new Exception("Inner exception")));

        var middleware = new GlobalExceptionHandlerMiddleware(nextMock.Object, loggerMock.Object);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        // Verify that the response is set to 500 Internal Server Error
        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);

        // Verify that the response body contains the correct error message
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        Assert.Contains("An internal error occurred. Please try again later.", responseBody);

        // Verify the outer and inner exception were logged
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        loggerMock.Verify(
            logger =>
            logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Outer exception") && o.ToString().Contains("Inner exception")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
    }

    [Fact]
    public async Task InvokeAsync_DoesNotLogOrModifyResponseIfNoExceptionOccurs()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<GlobalExceptionHandlerMiddleware>>();
        var nextMock = new Mock<RequestDelegate>();
        nextMock
            .Setup(next => next(It.IsAny<HttpContext>()))
            .Returns(Task.CompletedTask);

        var middleware = new GlobalExceptionHandlerMiddleware(nextMock.Object, loggerMock.Object);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

        // Assert
        // Verify that the response status code and body remain untouched
        Assert.Equal(200, context.Response.StatusCode); // Default HTTP status code
        Assert.Empty(await new StreamReader(context.Response.Body).ReadToEndAsync());

        // Verify that no error was logged
        loggerMock.Verify(
            static logger =>
            logger.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<object>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()),
            Times.Never);
    }
}
