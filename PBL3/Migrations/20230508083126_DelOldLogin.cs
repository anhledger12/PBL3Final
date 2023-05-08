using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3.Migrations
{
    /// <inheritdoc />
    public partial class DelOldLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountLogin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountLogin",
                columns: table => new
                {
                    AccName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PassHashCode = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Permission = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AccountL__09224552BE35A89A", x => x.AccName);
                    table.ForeignKey(
                        name: "FK__AccountLo__Permi__38996AB5",
                        column: x => x.AccName,
                        principalTable: "Account",
                        principalColumn: "AccName");
                });
        }
    }
}
