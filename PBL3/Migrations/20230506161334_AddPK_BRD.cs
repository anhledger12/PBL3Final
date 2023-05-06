using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3.Migrations
{
    /// <inheritdoc />
    public partial class AddPK_BRD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IdBookRent_IdBook",
                table: "BookRentDetail");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BookRentDetail",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Id_BookRentDetail",
                table: "BookRentDetail",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BookRentDetail_IdBookRental",
                table: "BookRentDetail",
                column: "IdBookRental");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Id_BookRentDetail",
                table: "BookRentDetail");

            migrationBuilder.DropIndex(
                name: "IX_BookRentDetail_IdBookRental",
                table: "BookRentDetail");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BookRentDetail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdBookRent_IdBook",
                table: "BookRentDetail",
                columns: new[] { "IdBookRental", "IdBook" });
        }
    }
}
