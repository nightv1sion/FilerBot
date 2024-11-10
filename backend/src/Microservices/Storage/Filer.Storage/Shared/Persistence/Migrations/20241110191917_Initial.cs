using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filer.Storage.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "directories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    path = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    parent_directory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_directories", x => x.id);
                    table.ForeignKey(
                        name: "fk_directories_directories_parent_directory_id",
                        column: x => x.parent_directory_id,
                        principalTable: "directories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    path = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    extension = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    parent_directory_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_files", x => x.id);
                    table.ForeignKey(
                        name: "fk_files_directories_parent_directory_id",
                        column: x => x.parent_directory_id,
                        principalTable: "directories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_directories_parent_directory_id",
                table: "directories",
                column: "parent_directory_id");

            migrationBuilder.CreateIndex(
                name: "ix_files_parent_directory_id",
                table: "files",
                column: "parent_directory_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "directories");
        }
    }
}
