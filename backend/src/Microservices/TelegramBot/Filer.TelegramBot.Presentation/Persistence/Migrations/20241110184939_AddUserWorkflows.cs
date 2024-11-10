using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filer.TelegramBot.Presentation.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddUserWorkflows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "workflow",
                table: "user_states");

            migrationBuilder.DropColumn(
                name: "workflow_data",
                table: "user_states");

            migrationBuilder.DropColumn(
                name: "workflow_step",
                table: "user_states");

            migrationBuilder.AddColumn<Guid>(
                name: "current_workflow_id",
                table: "user_states",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "user_workflows",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    workflow_payload = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_workflows", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_user_states_current_workflow_id",
                table: "user_states",
                column: "current_workflow_id");

            migrationBuilder.AddForeignKey(
                name: "fk_user_states_user_workflows_current_workflow_id",
                table: "user_states",
                column: "current_workflow_id",
                principalTable: "user_workflows",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_user_states_user_workflows_current_workflow_id",
                table: "user_states");

            migrationBuilder.DropTable(
                name: "user_workflows");

            migrationBuilder.DropIndex(
                name: "ix_user_states_current_workflow_id",
                table: "user_states");

            migrationBuilder.DropColumn(
                name: "current_workflow_id",
                table: "user_states");

            migrationBuilder.AddColumn<string>(
                name: "workflow",
                table: "user_states",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "workflow_data",
                table: "user_states",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "workflow_step",
                table: "user_states",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }
    }
}
