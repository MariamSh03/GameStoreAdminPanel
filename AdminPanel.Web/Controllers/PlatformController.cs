using AdminPanel.Bll.DTOs;
using AdminPanel.Bll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Web.Controllers;

[ApiController]
[Route("platforms/[controller]")]
public class PlatformController : ControllerBase
{
    private readonly IPlatformService _platformService;

    public PlatformController(IPlatformService platformService)
    {
        _platformService = platformService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlatformDto>>> GetAllPlatforms()
    {
        try
        {
            var platforms = await _platformService.GetAllPlatformsAsync();
            return Ok(platforms);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlatformDto platformDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _platformService.AddPlatformAsync(platformDto);
            return Ok("Platform created successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PlatformDto platformDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _platformService.UpdatePlatformAsync(id, platformDto);
            return Ok("Platform updated successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _platformService.DeletePlatformAsync(id);
            return Ok("Platform deleted successfully.");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("/games/{key}/platforms")]
    public async Task<ActionResult<IEnumerable<PlatformDto>>> GetPlatformsByGameKey(string key)
    {
        try
        {
            var platforms = await _platformService.GetPlatformsByGameKeyAsync(key);
            return platforms == null || !platforms.Any() ? (ActionResult<IEnumerable<PlatformDto>>)NotFound($"No platforms found for game key: {key}") : Ok(platforms);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}