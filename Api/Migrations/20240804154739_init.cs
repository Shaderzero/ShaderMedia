using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Books");

            migrationBuilder.CreateTable(
                name: "Authors",
                schema: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    MiddleName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                schema: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                schema: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Series",
                schema: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                schema: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    SeriesId = table.Column<int>(type: "integer", nullable: true),
                    SeriesNo = table.Column<int>(type: "integer", nullable: true),
                    LibRate = table.Column<int>(type: "integer", nullable: true),
                    ZipName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    File = table.Column<int>(type: "integer", nullable: false),
                    Size = table.Column<int>(type: "integer", nullable: false),
                    Del = table.Column<bool>(type: "boolean", nullable: false),
                    Ext = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    LanguageId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalSchema: "Books",
                        principalTable: "Languages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Books_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalSchema: "Books",
                        principalTable: "Series",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BookAuthor",
                schema: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    AuthorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthor", x => new { x.BookId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_BookAuthor_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "Books",
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookAuthor_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "Books",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookGenre",
                schema: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenre", x => new { x.BookId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_BookGenre_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "Books",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenre_Genres_GenreId",
                        column: x => x.GenreId,
                        principalSchema: "Books",
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookKeyword",
                schema: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "integer", nullable: false),
                    KeywordId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookKeyword", x => new { x.BookId, x.KeywordId });
                    table.ForeignKey(
                        name: "FK_BookKeyword_Books_BookId",
                        column: x => x.BookId,
                        principalSchema: "Books",
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookKeyword_Keywords_KeywordId",
                        column: x => x.KeywordId,
                        principalSchema: "Books",
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_LastName",
                schema: "Books",
                table: "Authors",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_LastName_FirstName_MiddleName",
                schema: "Books",
                table: "Authors",
                columns: new[] { "LastName", "FirstName", "MiddleName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthor_AuthorId",
                schema: "Books",
                table: "BookAuthor",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenre_GenreId",
                schema: "Books",
                table: "BookGenre",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_BookKeyword_KeywordId",
                schema: "Books",
                table: "BookKeyword",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_LanguageId",
                schema: "Books",
                table: "Books",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_SeriesId",
                schema: "Books",
                table: "Books",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Title",
                schema: "Books",
                table: "Books",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Name",
                schema: "Books",
                table: "Genres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_Name",
                schema: "Books",
                table: "Keywords",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Name",
                schema: "Books",
                table: "Languages",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Series_Name",
                schema: "Books",
                table: "Series",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthor",
                schema: "Books");

            migrationBuilder.DropTable(
                name: "BookGenre",
                schema: "Books");

            migrationBuilder.DropTable(
                name: "BookKeyword",
                schema: "Books");

            migrationBuilder.DropTable(
                name: "Authors",
                schema: "Books");

            migrationBuilder.DropTable(
                name: "Genres",
                schema: "Books");

            migrationBuilder.DropTable(
                name: "Books",
                schema: "Books");

            migrationBuilder.DropTable(
                name: "Keywords",
                schema: "Books");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "Books");

            migrationBuilder.DropTable(
                name: "Series",
                schema: "Books");
        }
    }
}
