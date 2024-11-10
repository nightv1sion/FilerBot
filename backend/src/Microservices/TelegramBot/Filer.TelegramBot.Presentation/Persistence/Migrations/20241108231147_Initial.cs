using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filer.TelegramBot.Presentation.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_states",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    workflow = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    workflow_step = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    workflow_data = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_states", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_states_user_id",
                table: "user_states",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_states");
        }
    }
}
