using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CCCD = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__092245520483F489", x => x.AccName);
                });

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

            migrationBuilder.CreateTable(
                name: "ActionLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Acc = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Time = table.Column<DateTime>(type: "datetime", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ActionLo__3214EC0770136D3A", x => x.Id);
                    table.ForeignKey(
                        name: "FK__ActionLog__Acc__3E52440B",
                        column: x => x.Acc,
                        principalTable: "Account",
                        principalColumn: "AccName");
                });

            migrationBuilder.CreateTable(
                name: "BookRental",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccApprove = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    AccSending = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TimeCreate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BookRent__3214EC0743132A74", x => x.Id);
                    table.ForeignKey(
                        name: "FK__BookRenta__AccAp__412EB0B6",
                        column: x => x.AccApprove,
                        principalTable: "Account",
                        principalColumn: "AccName");
                    table.ForeignKey(
                        name: "FK__BookRenta__AccSe__4222D4EF",
                        column: x => x.AccSending,
                        principalTable: "Account",
                        principalColumn: "AccName");
                });

            migrationBuilder.CreateTable(
                name: "Notificate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccReceive = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TimeSending = table.Column<DateTime>(type: "date", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateRead = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__3214EC077B06361B", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Notificat__AccRe__3B75D760",
                        column: x => x.AccReceive,
                        principalTable: "Account",
                        principalColumn: "AccName");
                });

            migrationBuilder.CreateTable(
                name: "Title",
                columns: table => new
                {
                    IdTitle = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    IdRepublish = table.Column<int>(type: "int", nullable: true),
                    NameBook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameWriter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "date", nullable: true),
                    NameBookshelf = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Title__2123011DEE7FEB85", x => x.IdTitle);
                    table.ForeignKey(
                        name: "FK__Title__IdRepubli__48CFD27E",
                        column: x => x.IdRepublish,
                        principalTable: "Republish",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    IdBook = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    IdTitle = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    StateRent = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Book__2756CBDBD68BBF8F", x => x.IdBook);
                    table.ForeignKey(
                        name: "FK__Book__IdTitle__5070F446",
                        column: x => x.IdTitle,
                        principalTable: "Title",
                        principalColumn: "IdTitle");
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

            migrationBuilder.CreateTable(
                name: "BookRentDetail",
                columns: table => new
                {
                    IdBookRental = table.Column<int>(type: "int", nullable: false),
                    IdBook = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    StateReturn = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    StateApprove = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    StateTake = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    ReturnDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdBookRent_IdBook", x => new { x.IdBookRental, x.IdBook });
                    table.ForeignKey(
                        name: "FK__BookRentD__IdBoo__5629CD9C",
                        column: x => x.IdBookRental,
                        principalTable: "BookRental",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__BookRentD__IdBoo__571DF1D5",
                        column: x => x.IdBook,
                        principalTable: "Book",
                        principalColumn: "IdBook");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionLog_Acc",
                table: "ActionLog",
                column: "Acc");

            migrationBuilder.CreateIndex(
                name: "IX_Book_IdTitle",
                table: "Book",
                column: "IdTitle");

            migrationBuilder.CreateIndex(
                name: "IX_BookRental_AccApprove",
                table: "BookRental",
                column: "AccApprove");

            migrationBuilder.CreateIndex(
                name: "IX_BookRental_AccSending",
                table: "BookRental",
                column: "AccSending");

            migrationBuilder.CreateIndex(
                name: "IX_BookRentDetail_IdBook",
                table: "BookRentDetail",
                column: "IdBook");

            migrationBuilder.CreateIndex(
                name: "IX_Hashtag_title_IdTitle",
                table: "Hashtag_title",
                column: "IdTitle");

            migrationBuilder.CreateIndex(
                name: "IX_Notificate_AccReceive",
                table: "Notificate",
                column: "AccReceive");

            migrationBuilder.CreateIndex(
                name: "IX_Title_IdRepublish",
                table: "Title",
                column: "IdRepublish");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountLogin");

            migrationBuilder.DropTable(
                name: "ActionLog");

            migrationBuilder.DropTable(
                name: "BookRentDetail");

            migrationBuilder.DropTable(
                name: "Hashtag_title");

            migrationBuilder.DropTable(
                name: "Notificate");

            migrationBuilder.DropTable(
                name: "BookRental");

            migrationBuilder.DropTable(
                name: "Book");

            migrationBuilder.DropTable(
                name: "Hashtag");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Title");

            migrationBuilder.DropTable(
                name: "Republish");
        }
    }
}
