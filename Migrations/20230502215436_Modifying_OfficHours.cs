using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace officehours_management.Migrations
{
    public partial class Modifying_OfficHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficeHourAttendances");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "OfficeHours");

            migrationBuilder.AddColumn<string>(
                name: "Day",
                table: "OfficeHours",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "EndHour",
                table: "OfficeHours",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "StartHour",
                table: "OfficeHours",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Day",
                table: "OfficeHours");

            migrationBuilder.DropColumn(
                name: "EndHour",
                table: "OfficeHours");

            migrationBuilder.DropColumn(
                name: "StartHour",
                table: "OfficeHours");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "OfficeHours",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "OfficeHourAttendances",
                columns: table => new
                {
                    AttendeeId = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OfficeHourId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeHourAttendances", x => new { x.AttendeeId, x.OfficeHourId });
                    table.ForeignKey(
                        name: "FK_OfficeHourAttendances_AspNetUsers_AttendeeId",
                        column: x => x.AttendeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficeHourAttendances_OfficeHours_OfficeHourId",
                        column: x => x.OfficeHourId,
                        principalTable: "OfficeHours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeHourAttendances_OfficeHourId",
                table: "OfficeHourAttendances",
                column: "OfficeHourId");
        }
    }
}
