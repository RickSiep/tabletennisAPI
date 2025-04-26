using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TableTennisAPI.Migrations
{
    /// <inheritdoc />
    public partial class ScoresInMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoserScore",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WinnerScore",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoserScore",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "WinnerScore",
                table: "Matches");
        }
    }
}
