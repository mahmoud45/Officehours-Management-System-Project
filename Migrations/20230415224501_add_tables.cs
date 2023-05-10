using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace officehours_management.Migrations
{
    public partial class add_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeHour_AspNetUsers_StaffId",
                table: "OfficeHour");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeHourAttendance_AspNetUsers_AttendeeId",
                table: "OfficeHourAttendance");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeHourAttendance_OfficeHour_OfficeHourId",
                table: "OfficeHourAttendance");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectStaff_Subject_SubjectId",
                table: "SubjectStaff");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subject",
                table: "Subject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficeHourAttendance",
                table: "OfficeHourAttendance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficeHour",
                table: "OfficeHour");

            migrationBuilder.RenameTable(
                name: "Subject",
                newName: "Subjects");

            migrationBuilder.RenameTable(
                name: "OfficeHourAttendance",
                newName: "OfficeHourAttendances");

            migrationBuilder.RenameTable(
                name: "OfficeHour",
                newName: "OfficeHours");

            migrationBuilder.RenameIndex(
                name: "IX_OfficeHourAttendance_OfficeHourId",
                table: "OfficeHourAttendances",
                newName: "IX_OfficeHourAttendances_OfficeHourId");

            migrationBuilder.RenameIndex(
                name: "IX_OfficeHour_StaffId",
                table: "OfficeHours",
                newName: "IX_OfficeHours_StaffId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficeHourAttendances",
                table: "OfficeHourAttendances",
                columns: new[] { "AttendeeId", "OfficeHourId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficeHours",
                table: "OfficeHours",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeHourAttendances_AspNetUsers_AttendeeId",
                table: "OfficeHourAttendances",
                column: "AttendeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeHourAttendances_OfficeHours_OfficeHourId",
                table: "OfficeHourAttendances",
                column: "OfficeHourId",
                principalTable: "OfficeHours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeHours_AspNetUsers_StaffId",
                table: "OfficeHours",
                column: "StaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectStaff_Subjects_SubjectId",
                table: "SubjectStaff",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeHourAttendances_AspNetUsers_AttendeeId",
                table: "OfficeHourAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeHourAttendances_OfficeHours_OfficeHourId",
                table: "OfficeHourAttendances");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficeHours_AspNetUsers_StaffId",
                table: "OfficeHours");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectStaff_Subjects_SubjectId",
                table: "SubjectStaff");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficeHours",
                table: "OfficeHours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficeHourAttendances",
                table: "OfficeHourAttendances");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Subject");

            migrationBuilder.RenameTable(
                name: "OfficeHours",
                newName: "OfficeHour");

            migrationBuilder.RenameTable(
                name: "OfficeHourAttendances",
                newName: "OfficeHourAttendance");

            migrationBuilder.RenameIndex(
                name: "IX_OfficeHours_StaffId",
                table: "OfficeHour",
                newName: "IX_OfficeHour_StaffId");

            migrationBuilder.RenameIndex(
                name: "IX_OfficeHourAttendances_OfficeHourId",
                table: "OfficeHourAttendance",
                newName: "IX_OfficeHourAttendance_OfficeHourId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subject",
                table: "Subject",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficeHour",
                table: "OfficeHour",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficeHourAttendance",
                table: "OfficeHourAttendance",
                columns: new[] { "AttendeeId", "OfficeHourId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeHour_AspNetUsers_StaffId",
                table: "OfficeHour",
                column: "StaffId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeHourAttendance_AspNetUsers_AttendeeId",
                table: "OfficeHourAttendance",
                column: "AttendeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeHourAttendance_OfficeHour_OfficeHourId",
                table: "OfficeHourAttendance",
                column: "OfficeHourId",
                principalTable: "OfficeHour",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectStaff_Subject_SubjectId",
                table: "SubjectStaff",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
