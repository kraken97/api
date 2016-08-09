using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Task2.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    PageId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    AddedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "strftime('%Y-%m-%d %H:%M:%S')"),
                    Content = table.Column<string>(nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    UrlName = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.PageId);
                });

            migrationBuilder.CreateTable(
                name: "NavLinks",
                columns: table => new
                {
                    NavLinkId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    PageId = table.Column<int>(nullable: true),
                    ParentLinkID = table.Column<int>(nullable: true),
                    Position = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavLinks", x => x.NavLinkId);
                    table.ForeignKey(
                        name: "FK_NavLinks_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RelatedPages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Page1Id = table.Column<int>(nullable: true),
                    Page2Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedPages", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RelatedPages_Pages_Page1Id",
                        column: x => x.Page1Id,
                        principalTable: "Pages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RelatedPages_Pages_Page2Id",
                        column: x => x.Page2Id,
                        principalTable: "Pages",
                        principalColumn: "PageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NavLinks_PageId",
                table: "NavLinks",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedPages_Page1Id",
                table: "RelatedPages",
                column: "Page1Id");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedPages_Page2Id",
                table: "RelatedPages",
                column: "Page2Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NavLinks");

            migrationBuilder.DropTable(
                name: "RelatedPages");

            migrationBuilder.DropTable(
                name: "Pages");
        }
    }
}
