using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS1.Migrations
{
    public partial class CourseDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.CreateTable(
                name: "CourseActivityDocument",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InternalName = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    CourseActivityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseActivityDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseActivityDocument_CourseActivity_CourseActivityId",
                        column: x => x.CourseActivityId,
                        principalTable: "CourseActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseActivityDocument_CourseActivityId",
                table: "CourseActivityDocument",
                column: "CourseActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseActivityDocument");

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActivityId = table.Column<int>(nullable: false),
                    CourseActivityId = table.Column<int>(nullable: true),
                    CourseId = table.Column<int>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Filepath = table.Column<string>(nullable: true),
                    ModuleId = table.Column<int>(nullable: true),
                    SubmissionTime = table.Column<DateTime>(nullable: false),
                    User = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_CourseActivity_CourseActivityId",
                        column: x => x.CourseActivityId,
                        principalTable: "CourseActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_CourseActivityId",
                table: "Exercises",
                column: "CourseActivityId");
        }
    }
}
