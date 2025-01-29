using System.Diagnostics.CodeAnalysis;
using AdminPanel.Dal.Context;
using AdminPanel.Entity;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Dal.Repositories.Implementations;

[ExcludeFromCodeCoverage]
public class GameRepository : GenericRepository<GameEntity>, IGameRepository
{
    private readonly ApplicationDbContext _context;

    public GameRepository(ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task DeleteAsync(string key)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.Key == key) ?? throw new ArgumentException("Game not found.");
        _context.Games.Remove(game);
        await _context.SaveChangesAsync();
    }

    public async Task AddGenresAsync(Guid gameId, List<Guid> genreIds)
    {
        foreach (var genreId in genreIds)
        {
            _context.GameGenres.Add(new GameGenreEntity
            {
                GameId = gameId,
                GenreId = genreId,
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task AddPlatformsAsync(Guid gameId, List<Guid> platformIds)
    {
        foreach (var platformId in platformIds)
        {
            _context.GamePlatforms.Add(new GamePlatformEntity
            {
                GameId = gameId,
                PlatformId = platformId,
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<GameEntity>> GetGamesByGenreAsync(Guid genreId)
    {
        return await _context.Games
            .Where(game => _context.GameGenres
                .Any(gg => gg.GameId == game.Id && gg.GenreId == genreId))
            .ToListAsync();
    }

    public async Task<IEnumerable<GameEntity>> GetGamesByPlatformAsync(Guid platformId)
    {
        return await _context.Games
            .Where(game => _context.GamePlatforms
                .Any(gg => gg.GameId == game.Id && gg.PlatformId == platformId))
            .ToListAsync();
    }

    public async Task<GameEntity> GetByKeyAsync(string key)
    {
        return await _context.Games.FirstOrDefaultAsync(g => g.Key == key);
    }

    public async Task<bool> DoesGenreExistAsync(Guid genreId)
    {
        return await _context.Genres.AnyAsync(genre => genre.Id == genreId);
    }

    public async Task<bool> DoesPlatformExistAsync(Guid platformId)
    {
        return await _context.Platforms.AnyAsync(platform => platform.Id == platformId);
    }
}
