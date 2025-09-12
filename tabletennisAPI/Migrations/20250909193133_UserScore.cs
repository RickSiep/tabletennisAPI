using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TableTennisAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "UserMatches");

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

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "UserMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
