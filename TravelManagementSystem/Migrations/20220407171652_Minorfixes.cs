using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelManagementSystem.Migrations
{
    public partial class Minorfixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Places",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Places_CityId",
                table: "Places",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Cities_CityId",
                table: "Places",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Cities_CityId",
                table: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Places_CityId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Images");
        }
    }
}
