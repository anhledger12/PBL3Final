using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3.Migrations
{
    /// <inheritdoc />
    public partial class AddCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountAccName = table.Column<string>(type: "varchar(50)", nullable: false),
                    TitleIdTitle = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cart_Account_AccountAccName",
                        column: x => x.AccountAccName,
                        principalTable: "Account",
                        principalColumn: "AccName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cart_Title_TitleIdTitle",
                        column: x => x.TitleIdTitle,
                        principalTable: "Title",
                        principalColumn: "IdTitle",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_AccountAccName",
                table: "Cart",
                column: "AccountAccName");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_TitleIdTitle",
                table: "Cart",
                column: "TitleIdTitle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cart");
        }
    }
}
