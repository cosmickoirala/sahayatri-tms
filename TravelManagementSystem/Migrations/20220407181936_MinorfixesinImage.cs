using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelManagementSystem.Migrations
{
    public partial class MinorfixesinImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_CityId",
                table: "Images",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Cities_CityId",
                table: "Images",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Cities_CityId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_CityId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Images");
        }
    }
}
