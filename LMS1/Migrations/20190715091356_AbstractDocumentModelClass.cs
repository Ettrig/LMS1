using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS1.Migrations
{
    public partial class AbstractDocumentModelClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Course_CourseId",
                table: "Document");

            migrationBuilder.DropTable(
                name: "CourseDocument");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Document");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "Document",
                newName: "CourseId1");

            migrationBuilder.RenameColumn(
                name: "Filepath",
                table: "Document",
                newName: "InternalName");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Document",
                nullable: false,
                defaultValue: "");

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
                name: "IX_Document_CourseId1",
                table: "Document",
                column: "CourseId1");

            migrationBuilder.CreateIndex(
                name: "IX_CourseActivityDocument_CourseActivityId",
                table: "CourseActivityDocument",
                column: "CourseActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Course_CourseId",
                table: "Document",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Course_CourseId1",
                table: "Document",
                column: "CourseId1",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Course_CourseId",
                table: "Document");

            migrationBuilder.DropForeignKey(
                name: "FK_Document_Course_CourseId1",
                table: "Document");

            migrationBuilder.DropTable(
                name: "CourseActivityDocument");

            migrationBuilder.DropIndex(
                name: "IX_Document_CourseId1",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Document");

            migrationBuilder.RenameColumn(
                name: "InternalName",
                table: "Document",
                newName: "Filepath");

            migrationBuilder.RenameColumn(
                name: "CourseId1",
                table: "Document",
                newName: "ModuleId");

            migrationBuilder.AddColumn<int>(
                name: "ActivityId",
                table: "Document",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Document",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseDocument",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    InternalName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseDocument_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_CourseDocument_CourseId",
                table: "CourseDocument",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_CourseActivityId",
                table: "Exercises",
                column: "CourseActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Course_CourseId",
                table: "Document",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
