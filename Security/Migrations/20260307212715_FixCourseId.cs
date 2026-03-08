using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Security.Migrations
{
    /// <inheritdoc />
    public partial class FixCourseId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoursedId",
                table: "Enrollments",
                newName: "CourseId");

            migrationBuilder.RenameColumn(
                name: "isPublished",
                table: "Courses",
                newName: "IsPublished");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Enrollments",
                newName: "CoursedId");

            migrationBuilder.RenameColumn(
                name: "IsPublished",
                table: "Courses",
                newName: "isPublished");
        }
    }
}
