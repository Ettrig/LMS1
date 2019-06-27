using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS1.Migrations
{
    public partial class courseisoptionalforuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Course_CourseId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Course_CourseId",
                table: "AspNetUsers",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Course_CourseId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Course_CourseId",
                table: "AspNetUsers",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
