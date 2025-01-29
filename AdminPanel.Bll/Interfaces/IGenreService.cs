using AdminPanel.Bll.DTOs;
using AdminPanel.Entity;

namespace AdminPanel.Bll.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreEntity>> GetAllGenresAsync();

    Task<GenreEntity> GetGenreByIdAsync(Guid id);

    Task AddGenreAsync(GenreDto genre);

    Task UpdateGenreAsync(Guid id, GenreDto genre);

    Task DeleteGenreAsync(Guid id);

    Task AddGenreAsync(string name);

    Task UpdateGenreAsync(string name);
}
