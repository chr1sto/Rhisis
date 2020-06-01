using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rhisis.Database.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    EmailConfirmed = table.Column<ulong>(type: "BIT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    LastConnectionTime = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Authority = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "characters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Gender = table.Column<byte>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Experience = table.Column<long>(nullable: false),
                    JobId = table.Column<int>(nullable: false),
                    Gold = table.Column<int>(nullable: false),
                    Slot = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    Strength = table.Column<int>(nullable: false),
                    Stamina = table.Column<int>(nullable: false),
                    Dexterity = table.Column<int>(nullable: false),
                    Intelligence = table.Column<int>(nullable: false),
                    Hp = table.Column<int>(nullable: false),
                    Mp = table.Column<int>(nullable: false),
                    Fp = table.Column<int>(nullable: false),
                    SkinSetId = table.Column<int>(nullable: false),
                    HairId = table.Column<int>(nullable: false),
                    HairColor = table.Column<int>(nullable: false),
                    FaceId = table.Column<int>(nullable: false),
                    MapId = table.Column<int>(nullable: false),
                    MapLayerId = table.Column<int>(nullable: false),
                    PosX = table.Column<float>(nullable: false),
                    PosY = table.Column<float>(nullable: false),
                    PosZ = table.Column<float>(nullable: false),
                    Angle = table.Column<float>(nullable: false),
                    BankCode = table.Column<short>(type: "SMALLINT(4)", nullable: false),
                    StatPoints = table.Column<int>(nullable: false),
                    SkillPoints = table.Column<int>(nullable: false),
                    LastConnectionTime = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    PlayTime = table.Column<long>(type: "BIGINT", nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_characters_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemId = table.Column<int>(nullable: false),
                    ItemCount = table.Column<int>(nullable: false),
                    ItemSlot = table.Column<int>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    Refine = table.Column<byte>(nullable: false),
                    Element = table.Column<byte>(nullable: false),
                    ElementRefine = table.Column<byte>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CharacterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_items_characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "quests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuestId = table.Column<int>(nullable: false),
                    Finished = table.Column<ulong>(type: "BIT", nullable: false),
                    IsChecked = table.Column<ulong>(type: "BIT", nullable: false),
                    IsDeleted = table.Column<ulong>(type: "BIT", nullable: false),
                    StartTime = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    IsPatrolDone = table.Column<ulong>(type: "BIT", nullable: false),
                    MonsterKilled1 = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    MonsterKilled2 = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    CharacterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_quests_characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shortcuts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TargetTaskbar = table.Column<int>(nullable: false),
                    SlotLevelIndex = table.Column<int>(nullable: true),
                    SlotIndex = table.Column<int>(nullable: false),
                    Type = table.Column<uint>(nullable: false),
                    ObjectId = table.Column<uint>(nullable: false),
                    ObjectType = table.Column<uint>(nullable: false),
                    ObjectIndex = table.Column<uint>(nullable: false),
                    UserId = table.Column<uint>(nullable: false),
                    ObjectData = table.Column<uint>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    CharacterId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SkillId = table.Column<int>(nullable: false),
                    Level = table.Column<byte>(nullable: false),
                    CharacterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Gold = table.Column<long>(type: "BIGINT", nullable: false),
                    ItemId = table.Column<int>(nullable: true),
                    ItemQuantity = table.Column<short>(nullable: false),
                    HasBeenRead = table.Column<bool>(nullable: false),
                    HasReceivedItem = table.Column<bool>(nullable: false),
                    HasReceivedGold = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    SenderId = table.Column<int>(nullable: false),
                    ReceiverId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mails_items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mails_characters_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mails_characters_SenderId",
                        column: x => x.SenderId,
                        principalTable: "characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_characters_UserId",
                table: "characters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_items_CharacterId",
                table: "items",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_mails_ItemId",
                table: "mails",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_mails_ReceiverId",
                table: "mails",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_mails_SenderId",
                table: "mails",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_quests_CharacterId",
                table: "quests",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_quests_QuestId_CharacterId",
                table: "quests",
                columns: new[] { "QuestId", "CharacterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shortcuts_CharacterId",
                table: "shortcuts",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_CharacterId",
                table: "Skills",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillId_CharacterId",
                table: "Skills",
                columns: new[] { "SkillId", "CharacterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Username_Email",
                table: "users",
                columns: new[] { "Username", "Email" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mails");

            migrationBuilder.DropTable(
                name: "quests");

            migrationBuilder.DropTable(
                name: "shortcuts");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "characters");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
