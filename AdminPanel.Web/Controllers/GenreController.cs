using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Web.Controllers;

[ApiController]
[Route("Genre/[controller]")]
public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    // GET: api/genres
    [HttpGet]
    public async Task<IActionResult> GetAllGenres()
    {
        var genres = await _genreService.GetAllGenresAsync();
        return Ok(genres);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GenreDto createGenreDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _genreService.AddGenreAsync(createGenreDto);
            return Ok("Genre created successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // GET: api/genres/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenreById(Guid id)
    {
        var genre = await _genreService.GetGenreByIdAsync(id);
        return genre == null ? NotFound($"Genre with ID {id} not found.") : Ok(genre);
    }

    // PUT: api/genres/{id}/update
    [HttpPut("{id}/update")]
    public async Task<IActionResult> UpdateGenre([FromBody] GenreDto genreDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _genreService.UpdateGenreAsync(genreDto.Name);
            return Ok("Genre updated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // DELETE: api/genres/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenre(Guid id)
    {
        try
        {
            await _genreService.DeleteGenreAsync(id);
            return Ok("Genre deleted successfully.");
        }
        catch (Exception ex)
        {
            return NotFound($"Genre with ID {id} not found: {ex.Message}");
        }
    }
}