using Microsoft.EntityFrameworkCore.Migrations;

namespace Rhisis.Database.Migrations
{
    public partial class v04x_ShortcutsReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shortcuts",
                columns: table => new
                {
                    Slot = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    SlotLevelIndex = table.Column<short>(type: "SMALLINT", nullable: false),
                    CharacterId = table.Column<int>(nullable: false),
                    TargetTaskbar = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    Type = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    ObjectType = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    ObjectItemSlot = table.Column<short>(type: "SMALLINT", nullable: true),
                    ObjectIndex = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    UserId = table.Column<short>(type: "SMALLINT", nullable: false),
                    ObjectData = table.Column<short>(type: "SMALLINT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shortcuts", x => new { x.CharacterId, x.Slot, x.SlotLevelIndex });
                    table.ForeignKey(
                        name: "FK_shortcuts_characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shortcuts_CharacterId_Slot_SlotLevelIndex",
                table: "shortcuts",
                columns: new[] { "CharacterId", "Slot", "SlotLevelIndex" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shortcuts");
        }
    }
}
