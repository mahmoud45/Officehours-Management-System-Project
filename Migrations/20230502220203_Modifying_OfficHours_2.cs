using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace officehours_management.Migrations
{
    public partial class Modifying_OfficHours_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StartHour",
                table: "OfficeHours",
                type: "varchar(45)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "EndHour",
                table: "OfficeHours",
                type: "varchar(45)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Day",
                table: "OfficeHours",
                type: "varchar(45)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StartHour",
                table: "OfficeHours",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "EndHour",
                table: "OfficeHours",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Day",
                table: "OfficeHours",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(45)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
