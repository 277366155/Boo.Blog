using Microsoft.EntityFrameworkCore.Migrations;

namespace Boo.Blog.EntityFrameworkCore.Migrations
{
    public partial class addDBDefaultField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "DatabaseServers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_DatabaseServerId",
                table: "Tenants",
                column: "DatabaseServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_DatabaseServers_DatabaseServerId",
                table: "Tenants",
                column: "DatabaseServerId",
                principalTable: "DatabaseServers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_DatabaseServers_DatabaseServerId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_DatabaseServerId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "DatabaseServers");
        }
    }
}
