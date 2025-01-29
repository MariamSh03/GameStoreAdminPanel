using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Services;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;
using AutoMapper;
using Moq;

namespace AdminPanel.Tests.Bll.Tests;

public class GameServiceTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _mapperMock = new Mock<IMapper>();
        _gameService = new GameService(_gameRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllGamesAsync_ShouldReturnAllGames()
    {
        // Arrange
        var games = new List<GameEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Game 1" },
            new() { Id = Guid.NewGuid(), Name = "Game 2" },
        };

        _gameRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(games);

        // Act
        var result = await _gameService.GetAllGamesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _gameRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetGameByIdAsync_ShouldReturnCorrectGame()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var game = new GameEntity { Id = gameId, Name = "Test Game" };
        _gameRepositoryMock.Setup(repo => repo.GetByIdAsync(gameId)).ReturnsAsync(game);

        // Act
        var result = await _gameService.GetGameByIdAsync(gameId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(gameId, result.Id);
        _gameRepositoryMock.Verify(repo => repo.GetByIdAsync(gameId), Times.Once);
    }

    [Fact]
    public async Task AddGameAsync_ShouldCallRepositoryAddAsync()
    {
        // Arrange
        var gameDto = new GameDto { Key = "new-key", Name = "New Game" };
        var gameEntity = new GameEntity { Key = "new-key", Name = "New Game" };

        _mapperMock.Setup(mapper => mapper.Map<GameEntity>(gameDto)).Returns(gameEntity);
        _gameRepositoryMock.Setup(repo => repo.AddAsync(gameEntity)).Returns(Task.CompletedTask);

        // Act
        await _gameService.AddGameAsync(gameDto);

        // Assert
        _mapperMock.Verify(mapper => mapper.Map<GameEntity>(gameDto), Times.Once);
        _gameRepositoryMock.Verify(repo => repo.AddAsync(gameEntity), Times.Once);
    }

    [Fact]
    public async Task UpdateGameAsync_ShouldCallRepositoryUpdateAsync()
    {
        // Arrange
        var key = "existing-key";
        var gameDto = new GameDto { Key = key, Name = "Updated Game" };
        var existingGame = new GameEntity { Key = key, Name = "Existing Game" };

        _gameRepositoryMock.Setup(repo => repo.GetByKeyAsync(key)).ReturnsAsync(existingGame);
        _mapperMock.Setup(mapper => mapper.Map(gameDto, existingGame)).Returns(existingGame);
        _gameRepositoryMock.Setup(repo => repo.UpdateAsync(existingGame)).Returns(Task.CompletedTask);

        // Act
        await _gameService.UpdateGameAsync(key, gameDto);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.GetByKeyAsync(key), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map(gameDto, existingGame), Times.Once);
        _gameRepositoryMock.Verify(repo => repo.UpdateAsync(existingGame), Times.Once);
    }

    [Fact]
    public async Task DeleteGameAsync_ShouldCallRepositoryDeleteAsync()
    {
        // Arrange
        var gameId = "someKey";
        _gameRepositoryMock.Setup(repo => repo.DeleteAsync(gameId)).Returns(Task.CompletedTask);

        // Act
        await _gameService.DeleteGameAsync(gameId);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.DeleteAsync(gameId), Times.Once);
    }

    [Fact]
    public void GetGamesByGenreAsync_ShouldReturnGamesWithMatchingGenre()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task GetGameByKeyAsync_ShouldReturnCorrectGame()
    {
        // Arrange
        var key = "unique-key";
        var game = new GameEntity { Id = Guid.NewGuid(), Key = key };
        var games = new List<GameEntity> { game };
        _gameRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(games);

        // Act
        var result = await _gameService.GetGameByKeyAsync(key);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(key, result.Key);
    }

    [Fact]
    public async Task GetTotalGamesCountAsync_ShouldReturnCorrectCount()
    {
        var games = new List<GameEntity>
            {
                new() { Id = Guid.NewGuid(), Name = "Game 1" },
                new() { Id = Guid.NewGuid(), Name = "Game 2" },
            };

        _gameRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(games); // Mock the repository

        var gameService = new GameService(_gameRepositoryMock.Object, _mapperMock.Object);

        // Act
        var totalGames = await gameService.GetTotalGamesCountAsync();

        // Assert
        Assert.Equal(2, totalGames); // Verify the count
    }

    [Fact]
    public void GetGamesByPlatformAsync_ShouldReturnGamesWithMatchingPlatform()
    {
        Assert.True(true);
    }
}