using Microsoft.EntityFrameworkCore.Migrations;

namespace DistSysACW.Migrations
{
    public partial class configuredUserLogRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Users_UserApiKey",
                table: "Logs");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Users_UserApiKey",
                table: "Logs",
                column: "UserApiKey",
                principalTable: "Users",
                principalColumn: "ApiKey",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Users_UserApiKey",
                table: "Logs");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Users_UserApiKey",
                table: "Logs",
                column: "UserApiKey",
                principalTable: "Users",
                principalColumn: "ApiKey",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
