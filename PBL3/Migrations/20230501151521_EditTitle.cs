using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3.Migrations
{
    /// <inheritdoc />
    public partial class EditTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "Title",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReleaseYear",
                table: "Title",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "Title");

            migrationBuilder.DropColumn(
                name: "ReleaseYear",
                table: "Title");
        }
    }
}
