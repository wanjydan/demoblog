using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoBlog.Migrations
{
    public partial class Image2Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                newName: "AppFeaturedImages");

            migrationBuilder.RenameIndex(
                name: "IX_AppFeaturedImage_UpdatedById",
                table: "AppFeaturedImages",
                newName: "IX_AppFeaturedImages_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_AppFeaturedImage_CreatedById",
                table: "AppFeaturedImages",
                newName: "IX_AppFeaturedImages_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_AppFeaturedImage_ArticleId",
                table: "AppFeaturedImages",
                newName: "IX_AppFeaturedImages_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppFeaturedImages",
                table: "AppFeaturedImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppFeaturedImages_AppArticles_ArticleId",
                table: "AppFeaturedImages",
                column: "ArticleId",
                principalTable: "AppArticles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppFeaturedImages_AspNetUsers_CreatedById",
                table: "AppFeaturedImages",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppFeaturedImages_AspNetUsers_UpdatedById",
                table: "AppFeaturedImages",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppFeaturedImages_AppArticles_ArticleId",
                table: "AppFeaturedImages");

            migrationBuilder.DropForeignKey(
                name: "FK_AppFeaturedImages_AspNetUsers_CreatedById",
                table: "AppFeaturedImages");

            migrationBuilder.DropForeignKey(
                name: "FK_AppFeaturedImages_AspNetUsers_UpdatedById",
                table: "AppFeaturedImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppFeaturedImages",
                table: "AppFeaturedImages");

            migrationBuilder.RenameTable(
                name: "AppFeaturedImages",
                newName: "AppFeaturedImage");

            migrationBuilder.RenameIndex(
                name: "IX_AppFeaturedImages_UpdatedById",
                table: "AppFeaturedImage",
                newName: "IX_AppFeaturedImage_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_AppFeaturedImages_CreatedById",
                table: "AppFeaturedImage",
                newName: "IX_AppFeaturedImage_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_AppFeaturedImages_ArticleId",
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
    }
}
