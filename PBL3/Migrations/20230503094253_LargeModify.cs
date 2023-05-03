using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3.Migrations
{
    /// <inheritdoc />
    public partial class LargeModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Title__IdRepubli__48CFD27E",
                table: "Title");

            migrationBuilder.DropTable(
                name: "Republish");

            migrationBuilder.DropIndex(
                name: "IX_Title_IdRepublish",
                table: "Title");

            migrationBuilder.DropColumn(
                name: "IdRepublish",
                table: "Title");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Title");

            migrationBuilder.DropColumn(
                name: "StateApprove",
                table: "BookRentDetail");

            migrationBuilder.AddColumn<bool>(
                name: "StateApprove",
                table: "BookRental",
                type: "bit",
                nullable: true,
                defaultValueSql: "((0))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StateApprove",
                table: "BookRental");

            migrationBuilder.AddColumn<int>(
                name: "IdRepublish",
                table: "Title",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Title",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "StateApprove",
                table: "BookRentDetail",
                type: "bit",
                nullable: true,
                defaultValueSql: "((0))");

            migrationBuilder.CreateTable(
                name: "Republish",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameRepublisher = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumOfRep = table.Column<int>(type: "int", nullable: true),
                    TimeOfRep = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Republis__3214EC076ADD85DF", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Title_IdRepublish",
                table: "Title",
                column: "IdRepublish");

            migrationBuilder.AddForeignKey(
                name: "FK__Title__IdRepubli__48CFD27E",
                table: "Title",
                column: "IdRepublish",
                principalTable: "Republish",
                principalColumn: "Id");
        }
    }
}
