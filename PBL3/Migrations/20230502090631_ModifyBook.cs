using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3.Migrations
{
    /// <inheritdoc />
    public partial class ModifyBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Book__IdTitle__5070F446",
                table: "Book");

            migrationBuilder.AlterColumn<string>(
                name: "IdTitle",
                table: "Book",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubId",
                table: "Book",
                type: "varchar(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK__Book__IdTitle__5070F446",
                table: "Book",
                column: "IdTitle",
                principalTable: "Title",
                principalColumn: "IdTitle",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Book__IdTitle__5070F446",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "SubId",
                table: "Book");

            migrationBuilder.AlterColumn<string>(
                name: "IdTitle",
                table: "Book",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AddForeignKey(
                name: "FK__Book__IdTitle__5070F446",
                table: "Book",
                column: "IdTitle",
                principalTable: "Title",
                principalColumn: "IdTitle");
        }
    }
}
