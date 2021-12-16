using Microsoft.EntityFrameworkCore.Migrations;

namespace NtrTrs.Migrations
{
    public partial class AddUserToEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "entries",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_entries_UserId",
                table: "entries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_entries_users_UserId",
                table: "entries",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_entries_users_UserId",
                table: "entries");

            migrationBuilder.DropIndex(
                name: "IX_entries_UserId",
                table: "entries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "entries");
        }
    }
}
