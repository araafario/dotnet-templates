#nullable disable

namespace DotnetTemplates.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "TestPersons",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false),
                Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TestPersons", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_TestPersons_Name",
            table: "TestPersons",
            column: "Name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.DropTable(
            name: "TestPersons");
}
