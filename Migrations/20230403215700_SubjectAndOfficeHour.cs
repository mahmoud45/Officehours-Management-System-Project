using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace officehours_management.Migrations
{
    public partial class SubjectAndOfficeHour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfficeHour",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    StaffId = table.Column<string>(type: "varchar(150)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeHour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeHour_AspNetUsers_StaffId",
                        column: x => x.StaffId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OfficeHourAttendance",
                columns: table => new
                {
                    AttendeeId = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OfficeHourId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeHourAttendance", x => new { x.AttendeeId, x.OfficeHourId });
                    table.ForeignKey(
                        name: "FK_OfficeHourAttendance_AspNetUsers_AttendeeId",
                        column: x => x.AttendeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficeHourAttendance_OfficeHour_OfficeHourId",
                        column: x => x.OfficeHourId,
                        principalTable: "OfficeHour",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SubjectStaff",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<string>(type: "varchar(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectStaff", x => new { x.SubjectId, x.StaffId });
                    table.ForeignKey(
                        name: "FK_SubjectStaff_AspNetUsers_StaffId",
                        column: x => x.StaffId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectStaff_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeHour_StaffId",
                table: "OfficeHour",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeHourAttendance_OfficeHourId",
                table: "OfficeHourAttendance",
                column: "OfficeHourId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectStaff_StaffId",
                table: "SubjectStaff",
                column: "StaffId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficeHourAttendance");

            migrationBuilder.DropTable(
                name: "SubjectStaff");

            migrationBuilder.DropTable(
                name: "OfficeHour");

            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}
