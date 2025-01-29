using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Entity;

public class PlatformEntity
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Type { get; set; }
}