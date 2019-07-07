using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS1.Migrations
{
    public partial class LmsName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LmsName",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LmsName",
                table: "AspNetUsers");
        }
    }
}
