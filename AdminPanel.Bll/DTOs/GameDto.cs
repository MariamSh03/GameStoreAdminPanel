namespace AdminPanel.Bll.DTOs;
public class GameDto
{
    public string Name { get; set; }

    public string Key { get; set; }

    public string? Description { get; set; }

    public List<Guid> GenreIds { get; set; } = new();

    public List<Guid> PlatformIds { get; set; } = new();
}