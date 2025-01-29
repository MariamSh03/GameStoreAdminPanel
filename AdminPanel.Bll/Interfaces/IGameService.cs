using AdminPanel.Bll.DTOs;
using AdminPanel.Entity;

namespace AdminPanel.Bll.Interfaces;

public interface IGameService
{
    Task<IEnumerable<GameEntity>> GetAllGamesAsync();

    Task<GameEntity> GetGameByIdAsync(Guid id);

    Task AddGameAsync(GameDto game);

    Task UpdateGameAsync(string key, GameDto game);

    Task DeleteGameAsync(string key);

    Task<Stream> GetGameFileAsync(string key);

    Task<IEnumerable<GameEntity>> GetGamesByGenreAsync(Guid genreId);

    Task<GameEntity> GetGameByKeyAsync(string key);

    Task<IEnumerable<GameEntity>> GetGamesByPlatformAsync(Guid platformId);

    Task<int> GetTotalGamesCountAsync();
}