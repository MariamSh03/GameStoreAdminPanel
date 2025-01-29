using System.Text.Json;
using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Exceptions;
using AdminPanel.Bll.Interfaces;
using AdminPanel.Dal.Repositories;
using AdminPanel.Entity;
using AutoMapper;

namespace AdminPanel.Bll.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IMapper _mapper;

    public GameService(IGameRepository gameRepository, IMapper mapper)
    {
        _gameRepository = gameRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GameEntity>> GetAllGamesAsync()
    {
        return await _gameRepository.GetAllAsync();
    }

    public async Task<GameEntity> GetGameByIdAsync(Guid id)
    {
        var game = await _gameRepository.GetByIdAsync(id)
            ?? throw new GameNotFoundException(id.ToString(), "Unable to update non-existent game.");
        return game;
    }

    public async Task AddGameAsync(GameDto game)
    {
        if (string.IsNullOrEmpty(game.Key))
        {
            game.Key = GenerateKeyFromName(game.Name);
        }

        foreach (var genreId in game.GenreIds)
        {
            if (!await _gameRepository.DoesGenreExistAsync(genreId))
            {
                throw new InvalidOperationException($"The Genre ID '{genreId}' does not exist.");
            }
        }

        foreach (var platformId in game.PlatformIds)
        {
            if (!await _gameRepository.DoesPlatformExistAsync(platformId))
            {
                throw new InvalidOperationException($"The Platform ID '{platformId}' does not exist.");
            }
        }

        var gameEntity = _mapper.Map<GameEntity>(game);
        await _gameRepository.AddAsync(gameEntity);

        // Assuming gameEntity is now added and has an ID
        await _gameRepository.AddGenresAsync(gameEntity.Id, game.GenreIds);
        await _gameRepository.AddPlatformsAsync(gameEntity.Id, game.PlatformIds);
    }

    public async Task UpdateGameAsync(string key, GameDto game)
    {
        var existingGame = await _gameRepository.GetByKeyAsync(key)
            ?? throw new GameNotFoundException(key, "Unable to update non-existent game.");

        // Map the DTO to the existing entity
        _mapper.Map(game, existingGame);

        // Save changes
        await _gameRepository.UpdateAsync(existingGame);
    }

    public async Task DeleteGameAsync(string key)
    {
        await _gameRepository.DeleteAsync(key);
    }

    public async Task<IEnumerable<GameEntity>> GetGamesByGenreAsync(Guid genreId)
    {
        // Check if the genre exists before fetching games
        return !await _gameRepository.DoesGenreExistAsync(genreId)
            ? throw new InvalidOperationException($"Genre with ID '{genreId}' does not exist.")
            : await _gameRepository.GetGamesByGenreAsync(genreId);
    }

    public async Task<GameEntity> GetGameByKeyAsync(string key)
    {
        var games = await _gameRepository.GetAllAsync();
        return games.FirstOrDefault(game => game.Key == key);
    }

    public async Task<IEnumerable<GameEntity>> GetGamesByPlatformAsync(Guid platformId)
    {
        return !await _gameRepository.DoesPlatformExistAsync(platformId)
            ? throw new InvalidOperationException($"Platform with ID '{platformId}' does not exist.")
            : await _gameRepository.GetGamesByPlatformAsync(platformId);
    }

    public static Task<Stream> GetGameFileAsync(string key, Exception gameNotFoundExeption)
    {
        throw new NotImplementedException();
    }

    public async Task<Stream> GetGameFileAsync(string key)
    {
        var game = await _gameRepository.GetByKeyAsync(key) ?? throw new GameNotFoundException(key, "Unable to update non-existent game.");
        var serializedGame = JsonSerializer.Serialize(game);
        _ = $"_{key}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.txt";
        var memoryStream = new MemoryStream();
        var writer = new StreamWriter(memoryStream);
        await writer.WriteAsync(serializedGame);
        await writer.FlushAsync();
        memoryStream.Position = 0;

        return memoryStream;
    }

    public async Task<int> GetTotalGamesCountAsync()
    {
        var games = await _gameRepository.GetAllAsync();
        return games.Count();
    }

    private static string GenerateKeyFromName(string name)
    {
        return name.ToLower().Replace(" ", "-");
    }
}