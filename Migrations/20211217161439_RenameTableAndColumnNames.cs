using Microsoft.EntityFrameworkCore.Migrations;

namespace NtrTrs.Migrations
{
    public partial class RenameTableAndColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accepted_entries_activities_ActivityId",
                table: "accepted_entries");

            migrationBuilder.DropForeignKey(
                name: "FK_accepted_entries_month_entries_MonthEntryId",
                table: "accepted_entries");

            migrationBuilder.DropForeignKey(
                name: "FK_activities_users_ManagerId",
                table: "activities");

            migrationBuilder.DropForeignKey(
                name: "FK_entries_activities_ActivityId",
                table: "entries");

            migrationBuilder.DropForeignKey(
                name: "FK_entries_month_entries_MonthEntryId",
                table: "entries");

            migrationBuilder.DropForeignKey(
                name: "FK_entries_users_UserId",
                table: "entries");

            migrationBuilder.DropForeignKey(
                name: "FK_month_entries_users_UserId",
                table: "month_entries");

            migrationBuilder.DropForeignKey(
                name: "FK_subactivities_activities_ActivityId",
                table: "subactivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_subactivities",
                table: "subactivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_entries",
                table: "entries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_activities",
                table: "activities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_month_entries",
                table: "month_entries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accepted_entries",
                table: "accepted_entries");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "subactivities",
                newName: "Subactivities");

            migrationBuilder.RenameTable(
                name: "entries",
                newName: "Entries");

            migrationBuilder.RenameTable(
                name: "activities",
                newName: "Activities");

            migrationBuilder.RenameTable(
                name: "month_entries",
                newName: "MonthEntries");

            migrationBuilder.RenameTable(
                name: "accepted_entries",
                newName: "AcceptedEntries");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "logged_in",
                table: "Users",
                newName: "LoggedIn");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "Subactivities",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Subactivities",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_subactivities_ActivityId",
                table: "Subactivities",
                newName: "IX_Subactivities_ActivityId");

            migrationBuilder.RenameColumn(
                name: "time",
                table: "Entries",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "subcode",
                table: "Entries",
                newName: "Subcode");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Entries",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Entries",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Entries",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_entries_UserId",
                table: "Entries",
                newName: "IX_Entries_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_entries_MonthEntryId",
                table: "Entries",
                newName: "IX_Entries_MonthEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_entries_ActivityId",
                table: "Entries",
                newName: "IX_Entries_ActivityId");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "Activities",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "budget",
                table: "Activities",
                newName: "Budget");

            migrationBuilder.RenameColumn(
                name: "active",
                table: "Activities",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Activities",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_activities_ManagerId",
                table: "Activities",
                newName: "IX_Activities_ManagerId");

            migrationBuilder.RenameColumn(
                name: "frozen",
                table: "MonthEntries",
                newName: "Frozen");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "MonthEntries",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "MonthEntries",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_month_entries_UserId",
                table: "MonthEntries",
                newName: "IX_MonthEntries_UserId");

            migrationBuilder.RenameColumn(
                name: "time",
                table: "AcceptedEntries",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AcceptedEntries",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_accepted_entries_MonthEntryId",
                table: "AcceptedEntries",
                newName: "IX_AcceptedEntries_MonthEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_accepted_entries_ActivityId",
                table: "AcceptedEntries",
                newName: "IX_AcceptedEntries_ActivityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subactivities",
                table: "Subactivities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Entries",
                table: "Entries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MonthEntries",
                table: "MonthEntries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AcceptedEntries",
                table: "AcceptedEntries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AcceptedEntries_Activities_ActivityId",
                table: "AcceptedEntries",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AcceptedEntries_MonthEntries_MonthEntryId",
                table: "AcceptedEntries",
                column: "MonthEntryId",
                principalTable: "MonthEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Users_ManagerId",
                table: "Activities",
                column: "ManagerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Activities_ActivityId",
                table: "Entries",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_MonthEntries_MonthEntryId",
                table: "Entries",
                column: "MonthEntryId",
                principalTable: "MonthEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Users_UserId",
                table: "Entries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthEntries_Users_UserId",
                table: "MonthEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subactivities_Activities_ActivityId",
                table: "Subactivities",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcceptedEntries_Activities_ActivityId",
                table: "AcceptedEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_AcceptedEntries_MonthEntries_MonthEntryId",
                table: "AcceptedEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Users_ManagerId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Activities_ActivityId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_MonthEntries_MonthEntryId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Users_UserId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthEntries_Users_UserId",
                table: "MonthEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Subactivities_Activities_ActivityId",
                table: "Subactivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subactivities",
                table: "Subactivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Entries",
                table: "Entries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MonthEntries",
                table: "MonthEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AcceptedEntries",
                table: "AcceptedEntries");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Subactivities",
                newName: "subactivities");

            migrationBuilder.RenameTable(
                name: "Entries",
                newName: "entries");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "activities");

            migrationBuilder.RenameTable(
                name: "MonthEntries",
                newName: "month_entries");

            migrationBuilder.RenameTable(
                name: "AcceptedEntries",
                newName: "accepted_entries");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "LoggedIn",
                table: "users",
                newName: "logged_in");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "subactivities",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "subactivities",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Subactivities_ActivityId",
                table: "subactivities",
                newName: "IX_subactivities_ActivityId");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "entries",
                newName: "time");

            migrationBuilder.RenameColumn(
                name: "Subcode",
                table: "entries",
                newName: "subcode");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "entries",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "entries",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "entries",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Entries_UserId",
                table: "entries",
                newName: "IX_entries_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Entries_MonthEntryId",
                table: "entries",
                newName: "IX_entries_MonthEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_Entries_ActivityId",
                table: "entries",
                newName: "IX_entries_ActivityId");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "activities",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Budget",
                table: "activities",
                newName: "budget");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "activities",
                newName: "active");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "activities",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_ManagerId",
                table: "activities",
                newName: "IX_activities_ManagerId");

            migrationBuilder.RenameColumn(
                name: "Frozen",
                table: "month_entries",
                newName: "frozen");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "month_entries",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "month_entries",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_MonthEntries_UserId",
                table: "month_entries",
                newName: "IX_month_entries_UserId");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "accepted_entries",
                newName: "time");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "accepted_entries",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_AcceptedEntries_MonthEntryId",
                table: "accepted_entries",
                newName: "IX_accepted_entries_MonthEntryId");

            migrationBuilder.RenameIndex(
                name: "IX_AcceptedEntries_ActivityId",
                table: "accepted_entries",
                newName: "IX_accepted_entries_ActivityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_subactivities",
                table: "subactivities",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_entries",
                table: "entries",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_activities",
                table: "activities",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_month_entries",
                table: "month_entries",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_accepted_entries",
                table: "accepted_entries",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_accepted_entries_activities_ActivityId",
                table: "accepted_entries",
                column: "ActivityId",
                principalTable: "activities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_accepted_entries_month_entries_MonthEntryId",
                table: "accepted_entries",
                column: "MonthEntryId",
                principalTable: "month_entries",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_activities_users_ManagerId",
                table: "activities",
                column: "ManagerId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_entries_activities_ActivityId",
                table: "entries",
                column: "ActivityId",
                principalTable: "activities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_entries_month_entries_MonthEntryId",
                table: "entries",
                column: "MonthEntryId",
                principalTable: "month_entries",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_entries_users_UserId",
                table: "entries",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_month_entries_users_UserId",
                table: "month_entries",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_subactivities_activities_ActivityId",
                table: "subactivities",
                column: "ActivityId",
                principalTable: "activities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
