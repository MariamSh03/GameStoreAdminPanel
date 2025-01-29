using System.Text;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Entity;
using AdminPanel.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AdminPanel.Tests.Controller.Tests;

#pragma warning disable CA1001 // Types that own disposable fields should be disposable
public class GameControllerTests
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
{
    private readonly Mock<IGameService> _mockGameService;
    private readonly GameController _controller;

    public GameControllerTests()
    {
        _mockGameService = new Mock<IGameService>();
        _controller = new GameController(_mockGameService.Object);
    }

    [Fact]
    public async Task Index_ReturnsOkResult_WithListOfGames()
    {
        // Arrange
        var expectedGames = new List<GameEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Test Game 1", Key = "test-game-1" },
                new() { Id = Guid.NewGuid(), Name = "Test Game 2", Key = "test-game-2" },
            };
        _mockGameService.Setup(s => s.GetAllGamesAsync())
            .ReturnsAsync(expectedGames);

        // Act
        var result = await _controller.Index();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGames = Assert.IsAssignableFrom<IEnumerable<GameEntity>>(okResult.Value);
        Assert.Equal(expectedGames.Count, returnedGames.Count());
    }

    [Fact]
    public async Task Create_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var gameDto = new GameDto
        {
            Name = "New Game",
            Key = "new-game",
            Description = "Test Description",
            GenreIds = new List<Guid> { Guid.NewGuid() },
            PlatformIds = new List<Guid> { Guid.NewGuid() },
        };

        _mockGameService.Setup(s => s.AddGameAsync(It.IsAny<GameDto>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(gameDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Game created successfully.", okResult.Value);
    }

    [Fact]
    public async Task UpdateGame_WithValidModel_ReturnsOkResult()
    {
        // Arrange
        var key = "test-game";
        var gameDto = new GameDto
        {
            Name = "Updated Game",
            Key = "updated-game",
            Description = "Updated Description",
        };

        _mockGameService.Setup(s => s.UpdateGameAsync(key, gameDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateGame(key, gameDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Game updated successfully.", okResult.Value);
    }

    [Fact]
    public async Task UpdateGame_WhenExceptionOccurs_ReturnsStatusCode500()
    {
        // Arrange
        var key = "test-game";
        var gameDto = new GameDto();
        _mockGameService.Setup(s => s.UpdateGameAsync(key, gameDto))
            .ThrowsAsync(new Exception("Test exception"));

        // Act
        var result = await _controller.UpdateGame(key, gameDto);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task GetGameByKey_WhenGameExists_ReturnsOkResult()
    {
        // Arrange
        var key = "test-game";
        var game = new GameEntity { Id = Guid.NewGuid(), Name = "Test Game", Key = key };
        _mockGameService.Setup(s => s.GetGameByKeyAsync(key))
            .ReturnsAsync(game);

        // Act
        var result = await _controller.GetGameByKey(key);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGame = Assert.IsType<GameEntity>(okResult.Value);
        Assert.Equal(game.Key, returnedGame.Key);
    }

    [Fact]
    public async Task DeleteGameByKey_WhenGameExists_ReturnsOkResult()
    {
        // Arrange
        var key = "test-game";
        _mockGameService.Setup(s => s.DeleteGameAsync(key))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteGameByKey(key);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Deleted successfully", okResult.Value);
    }

    [Fact]
    public async Task GetGamesByGenre_WithValidGenreId_ReturnsOkResult()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var expectedGames = new List<GameEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Test Game 1" },
                new() { Id = Guid.NewGuid(), Name = "Test Game 2" },
            };
        _mockGameService.Setup(s => s.GetGamesByGenreAsync(genreId))
            .ReturnsAsync(expectedGames);

        // Act
        var result = await _controller.GetGamesByGenre(genreId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGames = Assert.IsAssignableFrom<IEnumerable<GameEntity>>(okResult.Value);
        Assert.Equal(expectedGames.Count, returnedGames.Count());
    }

    [Fact]
    public async Task GetGamesByPlatform_WithValidPlatformId_ReturnsOkResult()
    {
        // Arrange
        var platformId = Guid.NewGuid();
        var expectedGames = new List<GameEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Test Game 1" },
                new() { Id = Guid.NewGuid(), Name = "Test Game 2" },
            };
        _mockGameService.Setup(s => s.GetGamesByPlatformAsync(platformId))
            .ReturnsAsync(expectedGames);

        // Act
        var result = await _controller.GetGamesByPlatform(platformId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedGames = Assert.IsAssignableFrom<IEnumerable<GameEntity>>(okResult.Value);
        Assert.Equal(expectedGames.Count, returnedGames.Count());
    }

    [Fact]
    public async Task DownloadGameFile_WhenGameExists_ReturnsFileResult()
    {
        // Arrange
        var key = "test-game";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes("test content"));
        _mockGameService.Setup(s => s.GetGameFileAsync(key))
            .ReturnsAsync(stream);

        // Act
        var result = await _controller.DownloadGameFile(key);

        // Assert
        var fileResult = Assert.IsType<FileStreamResult>(result);
        Assert.Equal("text/plain", fileResult.ContentType);
        Assert.Contains(key, fileResult.FileDownloadName);
    }

    [Fact]
    public async Task DownloadGameFile_WhenGameDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var key = "non-existent-game";
        _mockGameService.Setup(s => s.GetGameFileAsync(key))
            .ThrowsAsync(new Exception("Game not found"));

        // Act
        var result = await _controller.DownloadGameFile(key);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Game not found", notFoundResult.Value);
    }
}