using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Rhisis.Database.Migrations
{
    public partial class Shortcuts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shortcuts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CharacterId = table.Column<int>(nullable: false),
                    ObjectData = table.Column<uint>(nullable: false),
                    ObjectId = table.Column<uint>(nullable: false),
                    ObjectIndex = table.Column<uint>(nullable: false),
                    ObjectType = table.Column<uint>(nullable: false),
                    SlotIndex = table.Column<int>(nullable: false),
                    SlotLevelIndex = table.Column<int>(nullable: true),
                    TargetTaskbar = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Type = table.Column<uint>(nullable: false),
                    UserId = table.Column<uint>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shortcuts");
        }
    }
}
