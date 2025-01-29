using AdminPanel.Bll.DTOs;
using AdminPanel.Entity;

namespace AdminPanel.Bll.Interfaces;

public interface IPlatformService
{
    Task<IEnumerable<PlatformEntity>> GetAllPlatformsAsync();

    Task<PlatformEntity> GetPlatformByIdAsync(Guid id);

    Task AddPlatformAsync(PlatformDto platform);

    Task UpdatePlatformAsync(Guid id, PlatformDto platform);

    Task DeletePlatformAsync(Guid id);

    Task<IEnumerable<PlatformEntity>> GetPlatformsByGameKeyAsync(string key);
}
