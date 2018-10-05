using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DemoBlog.Migrations
{
    public partial class RemoveFeaturedImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppFeaturedImages");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "AppArticles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "AppArticles");

            migrationBuilder.CreateTable(
                name: "AppFeaturedImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ArticleId = table.Column<int>(nullable: false),
                    Caption = table.Column<string>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFeaturedImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppFeaturedImages_AppArticles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "AppArticles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppFeaturedImages_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppFeaturedImages_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppFeaturedImages_ArticleId",
                table: "AppFeaturedImages",
                column: "ArticleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppFeaturedImages_CreatedById",
                table: "AppFeaturedImages",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AppFeaturedImages_UpdatedById",
                table: "AppFeaturedImages",
                column: "UpdatedById");
        }
    }
}
