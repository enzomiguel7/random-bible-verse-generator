using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibleVerseGenerator.Migrations
{
    /// <inheritdoc />
    public partial class ReferenceAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Verses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Verses");
        }
    }
}
