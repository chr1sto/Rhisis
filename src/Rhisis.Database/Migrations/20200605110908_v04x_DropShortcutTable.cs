using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rhisis.Database.Migrations
{
    public partial class v04x_DropShortcutTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shortcuts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shortcuts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    ObjectData = table.Column<uint>(type: "int unsigned", nullable: false),
                    ObjectId = table.Column<uint>(type: "int unsigned", nullable: false),
                    ObjectIndex = table.Column<uint>(type: "int unsigned", nullable: false),
                    ObjectType = table.Column<uint>(type: "int unsigned", nullable: false),
                    SlotIndex = table.Column<int>(type: "int", nullable: false),
                    SlotLevelIndex = table.Column<int>(type: "int", nullable: true),
                    TargetTaskbar = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Type = table.Column<uint>(type: "int unsigned", nullable: false),
                    UserId = table.Column<uint>(type: "int unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shortcuts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shortcuts_characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shortcuts_CharacterId",
                table: "shortcuts",
                column: "CharacterId");
        }
    }
}
