using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TableTennisAPI.Migrations
{
    /// <inheritdoc />
    public partial class RefactorSingleDoubleRatingName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Elo",
                table: "Users",
                newName: "SinglesRating");

            migrationBuilder.AddColumn<int>(
                name: "DoublesRating",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RatingAfter",
                table: "UserMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RatingBefore",
                table: "UserMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RatingDelta",
                table: "UserMatches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoublesRating",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RatingAfter",
                table: "UserMatches");

            migrationBuilder.DropColumn(
                name: "RatingBefore",
                table: "UserMatches");

            migrationBuilder.DropColumn(
                name: "RatingDelta",
                table: "UserMatches");

            migrationBuilder.RenameColumn(
                name: "SinglesRating",
                table: "Users",
                newName: "Elo");
        }
    }
}
