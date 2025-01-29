using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Entity;

public class GenreEntity
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    public Guid? ParentGenreId { get; set; } // Optional (for subgenres)
}