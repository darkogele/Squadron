using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SquadronApi.Migrations
{
    public partial class userFfInFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "File",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_File_UserId",
                table: "File",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_Users_UserId",
                table: "File",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_Users_UserId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_UserId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "File");
        }
    }
}
