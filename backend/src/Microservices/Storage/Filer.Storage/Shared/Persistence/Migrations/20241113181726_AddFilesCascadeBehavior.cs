using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filer.Storage.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFilesCascadeBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_files_directories_parent_directory_id",
                table: "files");

            migrationBuilder.AddForeignKey(
                name: "fk_files_directories_parent_directory_id",
                table: "files",
                column: "parent_directory_id",
                principalTable: "directories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_files_directories_parent_directory_id",
                table: "files");

            migrationBuilder.AddForeignKey(
                name: "fk_files_directories_parent_directory_id",
                table: "files",
                column: "parent_directory_id",
                principalTable: "directories",
                principalColumn: "id");
        }
    }
}
