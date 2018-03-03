using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace referendusnetcore.Migrations
{
    public partial class Inheritance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddColumn<string>(
                name: "OAuthId",
                table: "Users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "References",
                columns: table => new
                {
                    Issue = table.Column<string>(nullable: true),
                    Journal = table.Column<string>(nullable: true),
                    Pages = table.Column<string>(nullable: true),
                    Volume = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Edition = table.Column<string>(nullable: true),
                    Publisher = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Discriminator = table.Column<string>(nullable: false),
                    Identifier = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    Tags = table.Column<List<string>>(nullable: true),
                    Title = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: false),
                    AccessDate = table.Column<DateTime>(nullable: true),
                    PublishDate = table.Column<DateTime>(nullable: true),
                    SiteTitle = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    ReferenceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Author_References_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "References",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Author_ReferenceId",
                table: "Author",
                column: "ReferenceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropTable(
                name: "References");

            migrationBuilder.DropColumn(
                name: "OAuthId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Users",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);
        }
    }
}
