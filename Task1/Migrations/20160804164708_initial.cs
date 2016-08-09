using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mvc.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Genre = table.Column<string>(maxLength: 30, nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Rating = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movie");
        }
    }
}
