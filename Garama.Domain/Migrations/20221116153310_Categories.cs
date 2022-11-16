using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Garama.Domain.Migrations
{
    public partial class Categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "LastAuth",
            //    table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "AuthMethodImmutableIdSent",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "LastAuthDate",
            //    table: "Users",
            //    type: "datetime2",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropColumn(
                name: "AuthMethodImmutableIdSent",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastAuthDate",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastAuth",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
