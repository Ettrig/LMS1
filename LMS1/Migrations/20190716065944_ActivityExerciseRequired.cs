using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS1.Migrations
{
    public partial class ActivityExerciseRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ExerciseSubmissionRequired",
                table: "CourseActivity",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExerciseSubmissionRequired",
                table: "CourseActivity");
        }
    }
}
