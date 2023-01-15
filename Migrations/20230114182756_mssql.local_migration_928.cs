using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class mssqllocal_migration_928 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnonymousUserAnonId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_AnonymousUserAnonId",
                table: "CartItems",
                column: "AnonymousUserAnonId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_AnonymousUsers_AnonymousUserAnonId",
                table: "CartItems",
                column: "AnonymousUserAnonId",
                principalTable: "AnonymousUsers",
                principalColumn: "AnonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_AnonymousUsers_AnonymousUserAnonId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_AnonymousUserAnonId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "AnonymousUserAnonId",
                table: "CartItems");
        }
    }
}
