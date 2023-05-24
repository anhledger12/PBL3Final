using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hashtag_title");

            migrationBuilder.DropTable(
                name: "Hashtag");

            migrationBuilder.AddColumn<int>(
                name: "IdCategory",
                table: "Title",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameCategory = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__3214EC07C7662509", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Title_IdCategory",
                table: "Title",
                column: "IdCategory");

            migrationBuilder.AddForeignKey(
                name: "FK__titlecate__5629CD9C",
                table: "Title",
                column: "IdCategory",
                principalTable: "Category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__titlecate__5629CD9C",
                table: "Title");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Title_IdCategory",
                table: "Title");

            migrationBuilder.DropColumn(
                name: "IdCategory",
                table: "Title");

            migrationBuilder.CreateTable(
                name: "Hashtag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameHashTag = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Hashtag__3214EC07C7662509", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hashtag_title",
                columns: table => new
                {
                    IdHashtag = table.Column<int>(type: "int", nullable: false),
                    IdTitle = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ID", x => new { x.IdHashtag, x.IdTitle });
                    table.ForeignKey(
                        name: "FK__Hashtag_t__IdHas__4BAC3F29",
                        column: x => x.IdHashtag,
                        principalTable: "Hashtag",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Hashtag_t__IdTit__4CA06362",
                        column: x => x.IdTitle,
                        principalTable: "Title",
                        principalColumn: "IdTitle");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hashtag_title_IdTitle",
                table: "Hashtag_title",
                column: "IdTitle");
        }
    }
}
