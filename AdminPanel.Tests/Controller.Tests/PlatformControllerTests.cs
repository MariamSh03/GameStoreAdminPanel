using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdminPanel.Tests.Controller.Tests;
public class PlatformControllerTests
{
    private readonly Mock<IPlatformService> _mockPlatformService;
    private readonly PlatformController _controller;

    public PlatformControllerTests()
    {
        _mockPlatformService = new Mock<IPlatformService>();
        _controller = new PlatformController(_mockPlatformService.Object);
    }

    [Fact]
    public async Task GetAllPlatforms_WhenExceptionOccurs_ReturnsStatusCode500()
    {
        // Arrange
        _mockPlatformService.Setup(s => s.GetAllPlatformsAsync())
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetAllPlatforms();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Internal server error", statusCodeResult.Value.ToString());
    }

    [Fact]
    public async Task Create_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var platformDto = new PlatformDto { Type = "Mobile" };
        _mockPlatformService.Setup(s => s.AddPlatformAsync(It.IsAny<PlatformDto>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(platformDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Platform created successfully!", okResult.Value);
    }

    [Fact]
    public async Task Create_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var platformDto = new PlatformDto(); // Empty type
        _controller.ModelState.AddModelError("Type", "Type is required");

        // Act
        var result = await _controller.Create(platformDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_WhenExceptionOccurs_ReturnsStatusCode500()
    {
        // Arrange
        var platformDto = new PlatformDto { Type = "Mobile" };
        _mockPlatformService.Setup(s => s.AddPlatformAsync(It.IsAny<PlatformDto>()))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.Create(platformDto);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Internal server error", statusCodeResult.Value.ToString());
    }

    [Fact]
    public async Task Update_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var platformDto = new PlatformDto { Type = "Console" };
        _mockPlatformService.Setup(s => s.UpdatePlatformAsync(id, platformDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(id, platformDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Platform updated successfully.", okResult.Value);
    }

    [Fact]
    public async Task Update_WithInvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var id = Guid.NewGuid();
        var platformDto = new PlatformDto(); // Empty type
        _controller.ModelState.AddModelError("Type", "Type is required");

        // Act
        var result = await _controller.Update(id, platformDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_WhenPlatformNotFound_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var platformDto = new PlatformDto { Type = "Console" };
        _mockPlatformService.Setup(s => s.UpdatePlatformAsync(id, platformDto))
            .ThrowsAsync(new KeyNotFoundException("Platform not found"));

        // Act
        var result = await _controller.Update(id, platformDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("Platform not found", notFoundResult.Value.ToString());
    }

    [Fact]
    public async Task Delete_WhenPlatformExists_ReturnsOkResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockPlatformService.Setup(s => s.DeletePlatformAsync(id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Platform deleted successfully.", okResult.Value);
    }

    [Fact]
    public async Task Delete_WhenPlatformNotFound_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockPlatformService.Setup(s => s.DeletePlatformAsync(id))
            .ThrowsAsync(new KeyNotFoundException("Platform not found"));

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Contains("Platform not found", notFoundResult.Value.ToString());
    }

    [Fact]
    public async Task GetPlatformsByGameKey_WhenExceptionOccurs_ReturnsStatusCode500()
    {
        // Arrange
        var gameKey = "test-game";
        _mockPlatformService.Setup(s => s.GetPlatformsByGameKeyAsync(gameKey))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.GetPlatformsByGameKey(gameKey);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Internal server error", statusCodeResult.Value.ToString());
    }
}
