using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;

namespace AdminPanel.Bll.Services;

public class GenreService : IGenreService
{
    private readonly IGenericRepository<GenreEntity> _genreRepository;

    public GenreService(
        IGenericRepository<GenreEntity> genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task AddGenreAsync(GenreDto genre)
    {
        if (genre.ParentGenreId.HasValue)
        {
            _ = await _genreRepository.GetByIdAsync(genre.ParentGenreId.Value) ?? throw new ArgumentException($"Invalid ParentGenreId:{genre.ParentGenreId} No such genre exists.");
        }

        var genreEntity = new GenreEntity
        {
            Id = Guid.NewGuid(),
            Name = genre.Name,
            ParentGenreId = genre.ParentGenreId,
        };
        await _genreRepository.AddAsync(genreEntity);
    }

    public async Task AddGenreAsync(string name)
    {
        var genreEntity = new GenreEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
        };
        await _genreRepository.AddAsync(genreEntity);
    }

    public async Task<IEnumerable<GenreEntity>> GetAllGenresAsync()
    {
        return await _genreRepository.GetAllAsync();
    }

    public async Task<GenreEntity> GetGenreByIdAsync(Guid id)
    {
        return await _genreRepository.GetByIdAsync(id);
    }

    public async Task UpdateGenreAsync(Guid id, GenreDto genre)
    {
        var genreEntity = await _genreRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Genre not found");

        // Check if ParentGenreId is provided and is valid
        if (genre.ParentGenreId.HasValue)
        {
            _ = await _genreRepository.GetByIdAsync(genre.ParentGenreId.Value) ?? throw new ArgumentException($"Invalid ParentGenreId:{genre.ParentGenreId} No such genre exists.");
        }

        genreEntity.Name = genre.Name;
        genreEntity.ParentGenreId = genre.ParentGenreId;

        await _genreRepository.UpdateAsync(genreEntity);
    }

    public Task UpdateGenreAsync(string name)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteGenreAsync(Guid id)
    {
        var genre = await _genreRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Genre not found");
        await _genreRepository.DeleteAsync(genre);
    }
}