using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rhisis.MySQL.Migrations
{
    public partial class MultipleProvidersChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastConnectionTime",
                table: "users",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "users",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.AlterColumn<long>(
                name: "PlayTime",
                table: "characters",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "BIGINT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastConnectionTime",
                table: "characters",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastConnectionTime",
                table: "users",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "users",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<long>(
                name: "PlayTime",
                table: "characters",
                type: "BIGINT",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastConnectionTime",
                table: "characters",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
