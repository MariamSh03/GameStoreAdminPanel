using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Entity;

public class GamePlatformEntity
{
    [Required]
    public Guid GameId { get; set; }

    [Required]
    public Guid PlatformId { get; set; }
}
