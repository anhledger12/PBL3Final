using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3.Migrations
{
    /// <inheritdoc />
    public partial class AddActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Title",
                type: "bit",
                nullable: false,
                defaultValueSql: "((1))");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Book",
                type: "bit",
                nullable: false,
                defaultValueSql: "((1))");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Account",
                type: "bit",
                nullable: false,
                defaultValueSql: "((1))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Title");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Account");
        }
    }
}
