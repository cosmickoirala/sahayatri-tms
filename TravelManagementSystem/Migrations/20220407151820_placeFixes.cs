using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelManagementSystem.Migrations
{
    public partial class placeFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Countries_VisitedPlaceId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Countries_PlaceId",
                table: "Businesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Countries_CountryId",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_States_StateId",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Streets_StreetsId",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Countries_PlaceId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Countries_PlaceId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Countries_CountryId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_StateId",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_StreetsId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "PlaceName",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "PlaceType",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "StreetsId",
                table: "Countries");

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceType = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: true),
                    StreetsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Places_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Places_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Places_Streets_StreetsId",
                        column: x => x.StreetsId,
                        principalTable: "Streets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Places_CountryId",
                table: "Places",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_StateId",
                table: "Places",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Places_StreetsId",
                table: "Places",
                column: "StreetsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Places_VisitedPlaceId",
                table: "AspNetUsers",
                column: "VisitedPlaceId",
                principalTable: "Places",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Places_PlaceId",
                table: "Businesses",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Places_PlaceId",
                table: "Images",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Places_PlaceId",
                table: "Ratings",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Places_VisitedPlaceId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_Places_PlaceId",
                table: "Businesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Places_PlaceId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Places_PlaceId",
                table: "Ratings");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlaceName",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlaceType",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StateId",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StreetsId",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CountryId",
                table: "Countries",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_StateId",
                table: "Countries",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_StreetsId",
                table: "Countries",
                column: "StreetsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Countries_VisitedPlaceId",
                table: "AspNetUsers",
                column: "VisitedPlaceId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_Countries_PlaceId",
                table: "Businesses",
                column: "PlaceId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Countries_CountryId",
                table: "Countries",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_States_StateId",
                table: "Countries",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Streets_StreetsId",
                table: "Countries",
                column: "StreetsId",
                principalTable: "Streets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Countries_PlaceId",
                table: "Images",
                column: "PlaceId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Countries_PlaceId",
                table: "Ratings",
                column: "PlaceId",
                principalTable: "Countries",
                principalColumn: "Id");
        }
    }
}
