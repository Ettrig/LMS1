using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS1.Migrations
{
    public partial class RememberLastActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "CourseActivityId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CourseActivityId",
                table: "AspNetUsers",
                column: "CourseActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CourseActivity_CourseActivityId",
                table: "AspNetUsers",
                column: "CourseActivityId",
                principalTable: "CourseActivity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CourseActivity_CourseActivityId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CourseActivityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CourseActivityId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
