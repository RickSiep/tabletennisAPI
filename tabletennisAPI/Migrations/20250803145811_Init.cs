using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TableTennisAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMatch_Matches_MatchId",
                table: "UserMatch");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMatch_Users_UserId",
                table: "UserMatch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMatch",
                table: "UserMatch");

            migrationBuilder.RenameTable(
                name: "UserMatch",
                newName: "UserMatches");

            migrationBuilder.RenameIndex(
                name: "IX_UserMatch_UserId",
                table: "UserMatches",
                newName: "IX_UserMatches_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMatches",
                table: "UserMatches",
                columns: new[] { "MatchId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserMatches_Matches_MatchId",
                table: "UserMatches",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMatches_Users_UserId",
                table: "UserMatches",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMatches_Matches_MatchId",
                table: "UserMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMatches_Users_UserId",
                table: "UserMatches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMatches",
                table: "UserMatches");

            migrationBuilder.RenameTable(
                name: "UserMatches",
                newName: "UserMatch");

            migrationBuilder.RenameIndex(
                name: "IX_UserMatches_UserId",
                table: "UserMatch",
                newName: "IX_UserMatch_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMatch",
                table: "UserMatch",
                columns: new[] { "MatchId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserMatch_Matches_MatchId",
                table: "UserMatch",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserMatch_Users_UserId",
                table: "UserMatch",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
