using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Dal.Migrations;

public partial class InitialMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Games",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Games", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Genres",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ParentGenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Genres", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Platforms",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Platforms", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "GameGenres",
            columns: table => new
            {
                GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GameGenres", x => new { x.GameId, x.GenreId });
                table.ForeignKey(
                    name: "FK_GameGenres_Games_GameId",
                    column: x => x.GameId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_GameGenres_Genres_GenreId",
                    column: x => x.GenreId,
                    principalTable: "Genres",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "GamePlatforms",
            columns: table => new
            {
                GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PlatformId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_GamePlatforms", x => new { x.GameId, x.PlatformId });
                table.ForeignKey(
                    name: "FK_GamePlatforms_Games_GameId",
                    column: x => x.GameId,
                    principalTable: "Games",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_GamePlatforms_Platforms_PlatformId",
                    column: x => x.PlatformId,
                    principalTable: "Platforms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "Genres",
            columns: new[] { "Id", "Name", "ParentGenreId" },
            values: new object[,]
            {
                { new Guid("0dcc04f9-a4fd-4973-89df-7b438dcdbf75"), "FPS", new Guid("a8d4ec20-0105-4160-b3ae-35079c09014d") },
                { new Guid("2409f670-5ab8-4d2d-9d7d-d0eec9783bbc"), "RTS", new Guid("526d0fd8-cc70-4c02-ad59-038005e5fb9a") },
                { new Guid("2f4e1202-ad61-4bc9-917e-ce77186317ee"), "Off-road", new Guid("a1720151-33cd-42e0-949c-70587a6e4e92") },
                { new Guid("47dccd2a-c5c7-4810-9a7f-76fbc0f102ab"), "Arcade", new Guid("a1720151-33cd-42e0-949c-70587a6e4e92") },
                { new Guid("4daf3454-d58f-4f55-9b06-a5aac939866d"), "Formula", new Guid("a1720151-33cd-42e0-949c-70587a6e4e92") },
                { new Guid("526d0fd8-cc70-4c02-ad59-038005e5fb9a"), "Strategy", null },
                { new Guid("772cc8ed-7c40-446b-84cc-e634b810f80d"), "TBS", new Guid("526d0fd8-cc70-4c02-ad59-038005e5fb9a") },
                { new Guid("8a401a57-5661-447d-8861-88d1b05ca1f2"), "Sports", null },
                { new Guid("8ee402ca-f836-4c0f-b6e6-284d7d6daeea"), "TPS", new Guid("a8d4ec20-0105-4160-b3ae-35079c09014d") },
                { new Guid("978daae7-cc5b-4fe9-a9e2-2fa612d23225"), "Adventure", null },
                { new Guid("a1720151-33cd-42e0-949c-70587a6e4e92"), "Races", null },
                { new Guid("a8d4ec20-0105-4160-b3ae-35079c09014d"), "Action", null },
                { new Guid("b360d115-bbba-4672-af1a-d214271d8f6f"), "Puzzle & Skill", null },
                { new Guid("b3ad12e4-dd75-4d39-aac5-147bbc6ca337"), "RPG", null },
                { new Guid("d2fbe2f9-6985-4459-8c32-696fa8ef4059"), "Rally", new Guid("a1720151-33cd-42e0-949c-70587a6e4e92") },
            });

        migrationBuilder.InsertData(
            table: "Platforms",
            columns: new[] { "Id", "Type" },
            values: new object[,]
            {
                { new Guid("5aebf57f-5b0d-49e6-bcd8-3aeb223006cb"), "Mobile" },
                { new Guid("b6becb13-98ea-48a6-a5a2-2418ab89748a"), "Console" },
                { new Guid("b70d0a43-6ae4-40df-8f1a-c3f6265dc9bb"), "Desktop" },
                { new Guid("d9b40e7f-54a1-4529-85f8-08345ea44374"), "Browser" },
            });

        migrationBuilder.CreateIndex(
            name: "IX_GameGenres_GenreId",
            table: "GameGenres",
            column: "GenreId");

        migrationBuilder.CreateIndex(
            name: "IX_GamePlatforms_PlatformId",
            table: "GamePlatforms",
            column: "PlatformId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "GameGenres");

        migrationBuilder.DropTable(
            name: "GamePlatforms");

        migrationBuilder.DropTable(
            name: "Genres");

        migrationBuilder.DropTable(
            name: "Games");

        migrationBuilder.DropTable(
            name: "Platforms");
    }
}
