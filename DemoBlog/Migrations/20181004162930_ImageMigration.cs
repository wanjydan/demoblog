using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoBlog.Migrations
{
    public partial class ImageMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleImages_AppArticles_ArticleId",
                table: "ArticleImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleImages_AspNetUsers_CreatedById",
                table: "ArticleImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleImages_AspNetUsers_UpdatedById",
                table: "ArticleImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleImages",
                table: "ArticleImages");

            migrationBuilder.RenameTable(
                name: "ArticleImages",
                newName: "AppFeaturedImage");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleImages_UpdatedById",
                table: "AppFeaturedImage",
                newName: "IX_AppFeaturedImage_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleImages_CreatedById",
                table: "AppFeaturedImage",
                newName: "IX_AppFeaturedImage_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleImages_ArticleId",
                table: "AppFeaturedImage",
                newName: "IX_AppFeaturedImage_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppFeaturedImage",
                table: "AppFeaturedImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppFeaturedImage_AppArticles_ArticleId",
                table: "AppFeaturedImage",
                column: "ArticleId",
                principalTable: "AppArticles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppFeaturedImage_AspNetUsers_CreatedById",
                table: "AppFeaturedImage",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppFeaturedImage_AspNetUsers_UpdatedById",
                table: "AppFeaturedImage",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppFeaturedImage_AppArticles_ArticleId",
                table: "AppFeaturedImage");

            migrationBuilder.DropForeignKey(
                name: "FK_AppFeaturedImage_AspNetUsers_CreatedById",
                table: "AppFeaturedImage");

            migrationBuilder.DropForeignKey(
                name: "FK_AppFeaturedImage_AspNetUsers_UpdatedById",
                table: "AppFeaturedImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppFeaturedImage",
                table: "AppFeaturedImage");

            migrationBuilder.RenameTable(
                name: "AppFeaturedImage",
                newName: "ArticleImages");

            migrationBuilder.RenameIndex(
                name: "IX_AppFeaturedImage_UpdatedById",
                table: "ArticleImages",
                newName: "IX_ArticleImages_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_AppFeaturedImage_CreatedById",
                table: "ArticleImages",
                newName: "IX_ArticleImages_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_AppFeaturedImage_ArticleId",
                table: "ArticleImages",
                newName: "IX_ArticleImages_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleImages",
                table: "ArticleImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleImages_AppArticles_ArticleId",
                table: "ArticleImages",
                column: "ArticleId",
                principalTable: "AppArticles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleImages_AspNetUsers_CreatedById",
                table: "ArticleImages",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleImages_AspNetUsers_UpdatedById",
                table: "ArticleImages",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
