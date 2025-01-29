using AdminPanel.Entity;

namespace AdminPanel.Tests.Entity.Tests;
public class PlatformEntityTests
{
    [Fact]
    public void PlatformEntity_ShouldSetAndGetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var platform = new PlatformEntity
        {
            Id = id,
            Type = "PC",
        };

        // Assert
        Assert.Equal(id, platform.Id);
        Assert.Equal("PC", platform.Type);
    }
}