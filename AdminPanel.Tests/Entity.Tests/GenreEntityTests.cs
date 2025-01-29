using AdminPanel.Entity;

namespace AdminPanel.Tests.Entity.Tests;

public class GenreEntityTests
{
    [Fact]
    public void GenreEntity_ShouldSetAndGetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var parentGenreId = Guid.NewGuid();

        // Act
        var genre = new GenreEntity
        {
            Id = id,
            Name = "Action",
            ParentGenreId = parentGenreId,
        };

        // Assert
        Assert.Equal(id, genre.Id);
        Assert.Equal("Action", genre.Name);
        Assert.Equal(parentGenreId, genre.ParentGenreId);
    }

    [Fact]
    public void GenreEntity_ShouldAllowNullParentGenreId()
    {
        // Act
        var genre = new GenreEntity
        {
            Id = Guid.NewGuid(),
            Name = "Adventure",
            ParentGenreId = null,
        };

        // Assert
        Assert.Null(genre.ParentGenreId);
    }
}
