using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Services;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;
using Moq;

namespace AdminPanel.Tests.Bll.Tests;

public class GenreServiceTests
{
    private readonly Mock<IGenericRepository<GenreEntity>> _mockGenreRepository;
    private readonly GenreService _genreService;

    public GenreServiceTests()
    {
        _mockGenreRepository = new Mock<IGenericRepository<GenreEntity>>();
        _genreService = new GenreService(_mockGenreRepository.Object);
    }

    [Fact]
    public async Task AddGenreAsync_WithValidGenreDto_ShouldAddGenre()
    {
        // Arrange
        var genreDto = new GenreDto { Name = "Action", ParentGenreId = Guid.NewGuid() };
        var parentGenreEntity = new GenreEntity { Id = genreDto.ParentGenreId.Value, Name = "ParentGenre" };

        _mockGenreRepository.Setup(repo => repo.GetByIdAsync(genreDto.ParentGenreId.Value))
            .ReturnsAsync(parentGenreEntity);

        // Act
        await _genreService.AddGenreAsync(genreDto);

        // Assert
        _mockGenreRepository.Verify(repo => repo.AddAsync(It.Is<GenreEntity>(g => g.Name == genreDto.Name && g.ParentGenreId == genreDto.ParentGenreId)), Times.Once);
    }

    [Fact]
    public async Task AddGenreAsync_WithInvalidParentGenreId_ShouldThrowArgumentException()
    {
        // Arrange
        var genreDto = new GenreDto { Name = "Action", ParentGenreId = Guid.NewGuid() };

#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        _mockGenreRepository.Setup(repo => repo.GetByIdAsync(genreDto.ParentGenreId.Value))
        .ReturnsAsync((GenreEntity?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _genreService.AddGenreAsync(genreDto));
    }

    [Fact]
    public async Task AddGenreAsync_WithNameOnly_ShouldAddGenre()
    {
        // Arrange
        var name = "Action";

        // Act
        await _genreService.AddGenreAsync(name);

        // Assert
        _mockGenreRepository.Verify(repo => repo.AddAsync(It.Is<GenreEntity>(g => g.Name == name && g.ParentGenreId == null)), Times.Once);
    }

    [Fact]
    public async Task GetAllGenresAsync_ShouldReturnAllGenres()
    {
        // Arrange
        var genres = new List<GenreEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "Action" },
            new() { Id = Guid.NewGuid(), Name = "Comedy" },
        };

        _mockGenreRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(genres);

        // Act
        var result = await _genreService.GetAllGenresAsync();

        // Assert
        Assert.Equal(genres, result);
    }

    [Fact]
    public async Task GetGenreByIdAsync_WithValidId_ShouldReturnGenre()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var genreEntity = new GenreEntity { Id = genreId, Name = "Action" };

        _mockGenreRepository.Setup(repo => repo.GetByIdAsync(genreId))
            .ReturnsAsync(genreEntity);

        // Act
        var result = await _genreService.GetGenreByIdAsync(genreId);

        // Assert
        Assert.Equal(genreEntity, result);
    }

    [Fact]
    public async Task GetGenreByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var genreId = Guid.NewGuid();

        _mockGenreRepository.Setup(repo => repo.GetByIdAsync(genreId))
            .ReturnsAsync((GenreEntity)null);

        // Act
        var result = await _genreService.GetGenreByIdAsync(genreId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateGenreAsync_WithValidIdAndGenreDto_ShouldUpdateGenre()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var genreDto = new GenreDto { Name = "UpdatedAction", ParentGenreId = Guid.NewGuid() };
        var existingGenreEntity = new GenreEntity { Id = genreId, Name = "Action" };
        var parentGenreEntity = new GenreEntity { Id = genreDto.ParentGenreId.Value, Name = "ParentGenre" };

        _mockGenreRepository.Setup(repo => repo.GetByIdAsync(genreId))
            .ReturnsAsync(existingGenreEntity);

        _mockGenreRepository.Setup(repo => repo.GetByIdAsync(genreDto.ParentGenreId.Value))
            .ReturnsAsync(parentGenreEntity);

        // Act
        await _genreService.UpdateGenreAsync(genreId, genreDto);

        // Assert
        _mockGenreRepository.Verify(repo => repo.UpdateAsync(It.Is<GenreEntity>(g => g.Name == genreDto.Name && g.ParentGenreId == genreDto.ParentGenreId)), Times.Once);
    }

    [Fact]
    public async Task UpdateGenreAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var genreDto = new GenreDto { Name = "UpdatedAction", ParentGenreId = Guid.NewGuid() };

        _mockGenreRepository.Setup(repo => repo.GetByIdAsync(genreId))
            .ReturnsAsync((GenreEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _genreService.UpdateGenreAsync(genreId, genreDto));
    }

    [Fact]
    public async Task UpdateGenreAsync_WithInvalidParentGenreId_ShouldThrowArgumentException()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var genreDto = new GenreDto { Name = "UpdatedAction", ParentGenreId = Guid.NewGuid() };
        var existingGenreEntity = new GenreEntity { Id = genreId, Name = "Action" };

        _mockGenreRepository.Setup(repo => repo.GetByIdAsync(genreId))
            .ReturnsAsync(existingGenreEntity);

        _mockGenreRepository.Setup(repo => repo.GetByIdAsync(genreDto.ParentGenreId.Value))
            .ReturnsAsync((GenreEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _genreService.UpdateGenreAsync(genreId, genreDto));
    }

    [Fact]
    public async Task DeleteGenreAsync_WithValidId_ShouldDeleteGenre()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var genreEntity = new GenreEntity { Id = genreId, Name = "Action" };

        _mockGenreRepository.Setup(repo => repo.GetByIdAsync(genreId))
            .ReturnsAsync(genreEntity);

        // Act
        await _genreService.DeleteGenreAsync(genreId);

        // Assert
        _mockGenreRepository.Verify(repo => repo.DeleteAsync(genreEntity), Times.Once);
    }

    [Fact]
    public async Task DeleteGenreAsync_WithInvalidId_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var genreId = Guid.NewGuid();

        _mockGenreRepository.Setup(repo => repo.GetByIdAsync(genreId))
            .ReturnsAsync((GenreEntity)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _genreService.DeleteGenreAsync(genreId));
    }
}