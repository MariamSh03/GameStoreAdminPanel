using AdminPanel.Entity;

namespace AdminPanel.Tests.Entity.Tests;
public class GamePlatformEntityTests
{
    [Fact]
    public void GamePlatformEntity_ShouldSetAndGetPropertiesCorrectly()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var platformId = Guid.NewGuid();

        // Act
        var gamePlatform = new GamePlatformEntity
        {
            GameId = gameId,
            PlatformId = platformId,
        };

        // Assert
        Assert.Equal(gameId, gamePlatform.GameId);
        Assert.Equal(platformId, gamePlatform.PlatformId);
    }
}
