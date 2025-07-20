using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TableTennisAPI.Migrations
{
    /// <inheritdoc />
    public partial class RestructureMatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_MatchLoserID",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_MatchWinnerID",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_MatchLoserID",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_MatchWinnerID",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "LoserScore",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchLoserID",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "MatchWinnerID",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "WinnerScore",
                table: "Matches");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatePlayed",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "UserMatch",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMatch", x => new { x.MatchId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserMatch_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMatch_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Id",
                table: "Matches",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserMatch_UserId",
                table: "UserMatch",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMatch");

            migrationBuilder.DropIndex(
                name: "IX_Matches_Id",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "DatePlayed",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "LoserScore",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MatchLoserID",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MatchWinnerID",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WinnerScore",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_MatchLoserID",
                table: "Matches",
                column: "MatchLoserID");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_MatchWinnerID",
                table: "Matches",
                column: "MatchWinnerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_MatchLoserID",
                table: "Matches",
                column: "MatchLoserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_MatchWinnerID",
                table: "Matches",
                column: "MatchWinnerID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
