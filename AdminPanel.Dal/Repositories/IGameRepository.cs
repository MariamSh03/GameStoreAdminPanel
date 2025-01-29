using AdminPanel.Entity;

namespace AdminPanel.Dal.Repositories;

public interface IGameRepository : IGenericRepository<GameEntity>
{
    Task DeleteAsync(string key);

    Task AddGenresAsync(Guid gameId, List<Guid> genreIds);

    Task AddPlatformsAsync(Guid gameId, List<Guid> platformIds);

    Task<IEnumerable<GameEntity>> GetGamesByGenreAsync(Guid genreId);

    Task<IEnumerable<GameEntity>> GetGamesByPlatformAsync(Guid platformId);

    Task<GameEntity> GetByKeyAsync(string key);

    Task<bool> DoesGenreExistAsync(Guid genreId);

    Task<bool> DoesPlatformExistAsync(Guid platformId);
}
