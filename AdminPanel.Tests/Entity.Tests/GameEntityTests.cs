using AdminPanel.Entity;

namespace AdminPanel.Tests.Entity.Tests;

public class GameEntityTests
{
    [Fact]
    public void GameEntity_ShouldSetAndGetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var game = new GameEntity
        {
            Id = id,
            Name = "Super Game",
            Key = "super_game_key",
            Description = "An amazing game",
        };

        // Assert
        Assert.Equal(id, game.Id);
        Assert.Equal("Super Game", game.Name);
        Assert.Equal("super_game_key", game.Key);
        Assert.Equal("An amazing game", game.Description);
    }
}