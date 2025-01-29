using AdminPanel.Bll.Interfaces;
using AdminPanel.Web.Middleware;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AdminPanel.Tests.Midlware.Tests;
public class TotalGamesHeaderMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldAddTotalGamesHeader()
    {
        // Arrange
        var mockGameService = new Mock<IGameService>();
        mockGameService.Setup(service => service.GetTotalGamesCountAsync()).ReturnsAsync(10);

        var middleware = new TotalGamesHeaderMiddleware(next: (innerHttpContext) => Task.CompletedTask);

        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(httpContext, mockGameService.Object);

        // Assert
        Assert.True(httpContext.Response.Headers.ContainsKey("x-total-numbers-of-games"));
        Assert.Equal("10", httpContext.Response.Headers["x-total-numbers-of-games"]);
    }
}
