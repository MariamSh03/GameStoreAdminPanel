using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Entity;

public class GameGenreEntity
{
    [Required]
    public Guid GameId { get; set; }

    [Required]
    public Guid GenreId { get; set; }
}
