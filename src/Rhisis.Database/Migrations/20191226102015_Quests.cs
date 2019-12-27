using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rhisis.Database.MsySQL.Migrations
{
    public partial class Quests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_characters_users_UserId",
                table: "characters");

            migrationBuilder.DropForeignKey(
                name: "FK_items_characters_CharacterId",
                table: "items");

            migrationBuilder.DropForeignKey(
                name: "FK_mails_items_ItemId",
                table: "mails");

            migrationBuilder.DropForeignKey(
                name: "FK_mails_characters_ReceiverId",
                table: "mails");

            migrationBuilder.DropForeignKey(
                name: "FK_mails_characters_SenderId",
                table: "mails");

            migrationBuilder.DropForeignKey(
                name: "FK_shortcuts_characters_CharacterId",
                table: "shortcuts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_shortcuts",
                table: "shortcuts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mails",
                table: "mails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_items",
                table: "items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_characters",
                table: "characters");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "shortcuts",
                newName: "Shortcuts");

            migrationBuilder.RenameTable(
                name: "mails",
                newName: "Mails");

            migrationBuilder.RenameTable(
                name: "items",
                newName: "Items");

            migrationBuilder.RenameTable(
                name: "characters",
                newName: "Characters");

            migrationBuilder.RenameIndex(
                name: "IX_users_Username_Email",
                table: "Users",
                newName: "IX_Users_Username_Email");

            migrationBuilder.RenameIndex(
                name: "IX_shortcuts_CharacterId",
                table: "Shortcuts",
                newName: "IX_Shortcuts_CharacterId");

            migrationBuilder.RenameIndex(
                name: "IX_mails_SenderId",
                table: "Mails",
                newName: "IX_Mails_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_mails_ReceiverId",
                table: "Mails",
                newName: "IX_Mails_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_mails_ItemId",
                table: "Mails",
                newName: "IX_Mails_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_items_CharacterId",
                table: "Items",
                newName: "IX_Items_CharacterId");

            migrationBuilder.RenameIndex(
                name: "IX_characters_UserId",
                table: "Characters",
                newName: "IX_Characters_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shortcuts",
                table: "Shortcuts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mails",
                table: "Mails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                table: "Items",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Characters",
                table: "Characters",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Quests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuestId = table.Column<int>(nullable: false),
                    Finished = table.Column<ulong>(type: "BIT", nullable: false),
                    IsChecked = table.Column<ulong>(type: "BIT", nullable: false),
                    IsDeleted = table.Column<ulong>(type: "BIT", nullable: false),
                    StartTime = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    CharacterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quests_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestActions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    Index = table.Column<int>(nullable: true),
                    Value = table.Column<int>(nullable: false),
                    QuestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestActions_Quests_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestActions_QuestId",
                table: "QuestActions",
                column: "QuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_CharacterId",
                table: "Quests",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Quests_QuestId_CharacterId",
                table: "Quests",
                columns: new[] { "QuestId", "CharacterId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Users_UserId",
                table: "Characters",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Characters_CharacterId",
                table: "Items",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mails_Items_ItemId",
                table: "Mails",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mails_Characters_ReceiverId",
                table: "Mails",
                column: "ReceiverId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mails_Characters_SenderId",
                table: "Mails",
                column: "SenderId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shortcuts_Characters_CharacterId",
                table: "Shortcuts",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Users_UserId",
                table: "Characters");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Characters_CharacterId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Mails_Items_ItemId",
                table: "Mails");

            migrationBuilder.DropForeignKey(
                name: "FK_Mails_Characters_ReceiverId",
                table: "Mails");

            migrationBuilder.DropForeignKey(
                name: "FK_Mails_Characters_SenderId",
                table: "Mails");

            migrationBuilder.DropForeignKey(
                name: "FK_Shortcuts_Characters_CharacterId",
                table: "Shortcuts");

            migrationBuilder.DropTable(
                name: "QuestActions");

            migrationBuilder.DropTable(
                name: "Quests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shortcuts",
                table: "Shortcuts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mails",
                table: "Mails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Characters",
                table: "Characters");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Shortcuts",
                newName: "shortcuts");

            migrationBuilder.RenameTable(
                name: "Mails",
                newName: "mails");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "items");

            migrationBuilder.RenameTable(
                name: "Characters",
                newName: "characters");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Username_Email",
                table: "users",
                newName: "IX_users_Username_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Shortcuts_CharacterId",
                table: "shortcuts",
                newName: "IX_shortcuts_CharacterId");

            migrationBuilder.RenameIndex(
                name: "IX_Mails_SenderId",
                table: "mails",
                newName: "IX_mails_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Mails_ReceiverId",
                table: "mails",
                newName: "IX_mails_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Mails_ItemId",
                table: "mails",
                newName: "IX_mails_ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_CharacterId",
                table: "items",
                newName: "IX_items_CharacterId");

            migrationBuilder.RenameIndex(
                name: "IX_Characters_UserId",
                table: "characters",
                newName: "IX_characters_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_shortcuts",
                table: "shortcuts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mails",
                table: "mails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_items",
                table: "items",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_characters",
                table: "characters",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_characters_users_UserId",
                table: "characters",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_items_characters_CharacterId",
                table: "items",
                column: "CharacterId",
                principalTable: "characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mails_items_ItemId",
                table: "mails",
                column: "ItemId",
                principalTable: "items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_mails_characters_ReceiverId",
                table: "mails",
                column: "ReceiverId",
                principalTable: "characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mails_characters_SenderId",
                table: "mails",
                column: "SenderId",
                principalTable: "characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shortcuts_characters_CharacterId",
                table: "shortcuts",
                column: "CharacterId",
                principalTable: "characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
