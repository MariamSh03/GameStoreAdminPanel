using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Web.Controllers;

public class GameController : Controller
{
    private readonly IGameService _gameService;

    // Inject the service into the controller
    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("games")]
    public async Task<IActionResult> Index()
    {
        var games = await _gameService.GetAllGamesAsync();
        return Ok(games);
    }

    [HttpPost("games/create")]
    public async Task<IActionResult> Create([FromBody] GameDto createGameDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _gameService.AddGameAsync(createGameDto);

        return Ok("Game created successfully.");
    }

    [HttpPut("games/{key}/update")]
    public async Task<IActionResult> UpdateGame(string key, [FromBody] GameDto gameDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Update the game
            await _gameService.UpdateGameAsync(key, gameDto);

            return Ok("Game updated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("/games/{key}")]
    public async Task<IActionResult> GetGameByKey(string key)
    {
        var game = await _gameService.GetGameByKeyAsync(key);
        return game == null ? NotFound() : Ok(game);
    }

    [HttpDelete("games/{key}")]
    public async Task<IActionResult> DeleteGameByKey(string key)
    {
        try
        {
            await _gameService.DeleteGameAsync(key);
            return Ok("Deleted successfully");
        }
        catch (Exception ex)
        {
            // Log ex for internal tracking
            return NotFound($"Game with key {key} not found: {ex.Message}");
        }
    }

    [HttpGet("genres/{id}/games")]
    public async Task<IActionResult> GetGamesByGenre(Guid genreId)
    {
        try
        {
            var games = await _gameService.GetGamesByGenreAsync(genreId);
            return Ok(games);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // Handle invalid genre ID
        }
    }

    [HttpGet("platforms/{id}/games")]
    public async Task<IActionResult> GetGamesByPlatform(Guid platformId)
    {
        try
        {
            var games = await _gameService.GetGamesByPlatformAsync(platformId);
            return Ok(games);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // Handle invalid platform ID
        }
    }

    [HttpGet("/games/{key}/file")]
    public async Task<IActionResult> DownloadGameFile(string key)
    {
        try
        {
            var gameFileStream = await _gameService.GetGameFileAsync(key);
            var fileName = $"{key}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.txt";

            return File(gameFileStream, "text/plain", fileName);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message); // Return 404 if game is not found
        }
    }
}
