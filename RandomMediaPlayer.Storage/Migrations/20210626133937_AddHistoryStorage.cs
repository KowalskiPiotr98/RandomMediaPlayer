using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RandomMediaPlayer.Storage.Migrations
{
    public partial class AddHistoryStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UriHistory",
                columns: table => new
                {
                    BasePath = table.Column<string>(nullable: false),
                    EntityName = table.Column<string>(nullable: false),
                    AddedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UriHistory", x => new { x.BasePath, x.EntityName });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UriHistory");
        }
    }
}
