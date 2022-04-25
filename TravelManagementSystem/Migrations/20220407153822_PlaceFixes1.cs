using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelManagementSystem.Migrations
{
    public partial class PlaceFixes1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Streets_StreetsId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_States_StatesId",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_States_Cities_CitiesId",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_Countries_StatesId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "StatesId",
                table: "Countries");

            migrationBuilder.RenameColumn(
                name: "CitiesId",
                table: "States",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_States_CitiesId",
                table: "States",
                newName: "IX_States_CountryId");

            migrationBuilder.RenameColumn(
                name: "StreetsId",
                table: "Cities",
                newName: "StateId");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_StreetsId",
                table: "Cities",
                newName: "IX_Cities_StateId");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Streets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Streets_CityId",
                table: "Streets",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_States_StateId",
                table: "Cities",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_States_Countries_CountryId",
                table: "States",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Streets_Cities_CityId",
                table: "Streets",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_States_StateId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_States_Countries_CountryId",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_Streets_Cities_CityId",
                table: "Streets");

            migrationBuilder.DropIndex(
                name: "IX_Streets_CityId",
                table: "Streets");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Streets");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "States",
                newName: "CitiesId");

            migrationBuilder.RenameIndex(
                name: "IX_States_CountryId",
                table: "States",
                newName: "IX_States_CitiesId");

            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "Cities",
                newName: "StreetsId");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_StateId",
                table: "Cities",
                newName: "IX_Cities_StreetsId");

            migrationBuilder.AddColumn<int>(
                name: "StatesId",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_StatesId",
                table: "Countries",
                column: "StatesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Streets_StreetsId",
                table: "Cities",
                column: "StreetsId",
                principalTable: "Streets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_States_StatesId",
                table: "Countries",
                column: "StatesId",
                principalTable: "States",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_States_Cities_CitiesId",
                table: "States",
                column: "CitiesId",
                principalTable: "Cities",
                principalColumn: "Id");
        }
    }
}
