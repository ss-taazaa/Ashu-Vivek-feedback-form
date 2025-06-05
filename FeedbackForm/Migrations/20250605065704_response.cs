using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeedbackForm.Migrations
{
    /// <inheritdoc />
    public partial class response : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RespondentId",
                table: "Submissions",
                newName: "RespondentName");

            migrationBuilder.AddColumn<string>(
                name: "RespondentEmail",
                table: "Submissions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RespondentEmail",
                table: "Submissions");

            migrationBuilder.RenameColumn(
                name: "RespondentName",
                table: "Submissions",
                newName: "RespondentId");
        }
    }
}
