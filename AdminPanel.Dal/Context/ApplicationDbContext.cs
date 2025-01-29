using AdminPanel.Entity;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Dal.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<GameEntity> Games { get; set; }

    public DbSet<GenreEntity> Genres { get; set; }

    public DbSet<PlatformEntity> Platforms { get; set; }

    public DbSet<GameGenreEntity> GameGenres { get; set; }

    public DbSet<GamePlatformEntity> GamePlatforms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure many-to-many relationships
        modelBuilder.Entity<GameGenreEntity>()
            .HasKey(gg => new { gg.GameId, gg.GenreId });

        modelBuilder.Entity<GameGenreEntity>()
            .HasOne<GameEntity>()
            .WithMany()
            .HasForeignKey(gg => gg.GameId);

        modelBuilder.Entity<GameGenreEntity>()
            .HasOne<GenreEntity>()
            .WithMany()
            .HasForeignKey(gg => gg.GenreId);

        modelBuilder.Entity<GamePlatformEntity>()
            .HasKey(gp => new { gp.GameId, gp.PlatformId });

        modelBuilder.Entity<GamePlatformEntity>()
            .HasOne<GameEntity>()
            .WithMany()
            .HasForeignKey(gp => gp.GameId);

        modelBuilder.Entity<GamePlatformEntity>()
            .HasOne<PlatformEntity>()
            .WithMany()
            .HasForeignKey(gp => gp.PlatformId);

        // Seed predefined genres and subgenres
        var strategyId = Guid.NewGuid();
        var rtsId = Guid.NewGuid();
        var tbsId = Guid.NewGuid();
        var rpgId = Guid.NewGuid();
        var sportsId = Guid.NewGuid();
        var racesId = Guid.NewGuid();
        var rallyId = Guid.NewGuid();
        var arcadeId = Guid.NewGuid();
        var formulaId = Guid.NewGuid();
        var offRoadId = Guid.NewGuid();
        var actionId = Guid.NewGuid();
        var fpsId = Guid.NewGuid();
        var tpsId = Guid.NewGuid();
        var adventureId = Guid.NewGuid();
        var puzzleSkillId = Guid.NewGuid();

        modelBuilder.Entity<GenreEntity>().HasData(
            new GenreEntity { Id = strategyId, Name = "Strategy", ParentGenreId = null },
            new GenreEntity { Id = rpgId, Name = "RPG", ParentGenreId = null },
            new GenreEntity { Id = sportsId, Name = "Sports", ParentGenreId = null },
            new GenreEntity { Id = racesId, Name = "Races", ParentGenreId = null },
            new GenreEntity { Id = actionId, Name = "Action", ParentGenreId = null },
            new GenreEntity { Id = adventureId, Name = "Adventure", ParentGenreId = null },
            new GenreEntity { Id = puzzleSkillId, Name = "Puzzle & Skill", ParentGenreId = null },
            new GenreEntity { Id = rtsId, Name = "RTS", ParentGenreId = strategyId },
            new GenreEntity { Id = tbsId, Name = "TBS", ParentGenreId = strategyId },
            new GenreEntity { Id = rallyId, Name = "Rally", ParentGenreId = racesId },
            new GenreEntity { Id = arcadeId, Name = "Arcade", ParentGenreId = racesId },
            new GenreEntity { Id = formulaId, Name = "Formula", ParentGenreId = racesId },
            new GenreEntity { Id = offRoadId, Name = "Off-road", ParentGenreId = racesId },
            new GenreEntity { Id = fpsId, Name = "FPS", ParentGenreId = actionId },
            new GenreEntity { Id = tpsId, Name = "TPS", ParentGenreId = actionId });

        // Seed predefined platforms
        modelBuilder.Entity<PlatformEntity>().HasData(
            new PlatformEntity { Id = Guid.NewGuid(), Type = "Mobile" },
            new PlatformEntity { Id = Guid.NewGuid(), Type = "Desktop" },
            new PlatformEntity { Id = Guid.NewGuid(), Type = "Console" },
            new PlatformEntity { Id = Guid.NewGuid(), Type = "Browser" });
    }
}