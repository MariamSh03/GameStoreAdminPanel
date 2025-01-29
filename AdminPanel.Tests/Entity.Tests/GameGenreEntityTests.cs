using AdminPanel.Entity;

namespace AdminPanel.Tests.Entity.Tests;

public class GameGenreEntityTests
{
    [Fact]
    public void GameGenreEntity_ShouldSetAndGetPropertiesCorrectly()
    {
        // Arrange
        var gameId = Guid.NewGuid();
        var genreId = Guid.NewGuid();

        // Act
        var gameGenre = new GameGenreEntity
        {
            GameId = gameId,
            GenreId = genreId,
        };

        // Assert
        Assert.Equal(gameId, gameGenre.GameId);
        Assert.Equal(genreId, gameGenre.GenreId);
    }
}