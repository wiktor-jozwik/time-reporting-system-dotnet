using Microsoft.EntityFrameworkCore.Migrations;

namespace NtrTrs.Migrations
{
    public partial class AddUserToMonthEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "month_entries",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_month_entries_UserId",
                table: "month_entries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_month_entries_users_UserId",
                table: "month_entries",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_month_entries_users_UserId",
                table: "month_entries");

            migrationBuilder.DropIndex(
                name: "IX_month_entries_UserId",
                table: "month_entries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "month_entries");
        }
    }
}
