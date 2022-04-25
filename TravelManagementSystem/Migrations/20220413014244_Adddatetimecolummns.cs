using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelManagementSystem.Migrations
{
    public partial class Adddatetimecolummns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Businesses_BusinessesId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Places_VisitedPlaceId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BusinessesId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_VisitedPlaceId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BusinessesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VisitedPlaceId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "AddedById",
                table: "Streets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Streets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Streets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Streets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AddedById",
                table: "States",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "States",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "States",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "States",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Ratings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Ratings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AddedById",
                table: "Places",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Places",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Places",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Places",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AddedById",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Images",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Images",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Images",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AddedById",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Countries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Countries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AddedById",
                table: "Cities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Cities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Cities",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Cities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AddedBYId",
                table: "Businesses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Businesses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBYId",
                table: "Businesses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Businesses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Streets_AddedById",
                table: "Streets",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_UpdatedById",
                table: "Streets",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_States_AddedById",
                table: "States",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_States_UpdatedById",
                table: "States",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Places_AddedById",
                table: "Places",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Places_UpdatedById",
                table: "Places",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Images_AddedById",
                table: "Images",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Images_UpdatedById",
                table: "Images",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_AddedById",
                table: "Countries",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_UpdatedById",
                table: "Countries",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_AddedById",
                table: "Cities",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_UpdatedById",
                table: "Cities",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_AddedBYId",
                table: "Businesses",
                column: "AddedBYId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_UpdatedBYId",
                table: "Businesses",
                column: "UpdatedBYId");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_AspNetUsers_AddedBYId",
                table: "Businesses",
                column: "AddedBYId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Businesses_AspNetUsers_UpdatedBYId",
                table: "Businesses",
                column: "UpdatedBYId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_AspNetUsers_AddedById",
                table: "Cities",
                column: "AddedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_AspNetUsers_UpdatedById",
                table: "Cities",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_AddedById",
                table: "Countries",
                column: "AddedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_AspNetUsers_UpdatedById",
                table: "Countries",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_AspNetUsers_AddedById",
                table: "Images",
                column: "AddedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_AspNetUsers_UpdatedById",
                table: "Images",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_AspNetUsers_AddedById",
                table: "Places",
                column: "AddedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_AspNetUsers_UpdatedById",
                table: "Places",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_States_AspNetUsers_AddedById",
                table: "States",
                column: "AddedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_States_AspNetUsers_UpdatedById",
                table: "States",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Streets_AspNetUsers_AddedById",
                table: "Streets",
                column: "AddedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Streets_AspNetUsers_UpdatedById",
                table: "Streets",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_AspNetUsers_AddedBYId",
                table: "Businesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Businesses_AspNetUsers_UpdatedBYId",
                table: "Businesses");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_AspNetUsers_AddedById",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Cities_AspNetUsers_UpdatedById",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_AddedById",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Countries_AspNetUsers_UpdatedById",
                table: "Countries");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_AspNetUsers_AddedById",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_AspNetUsers_UpdatedById",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Places_AspNetUsers_AddedById",
                table: "Places");

            migrationBuilder.DropForeignKey(
                name: "FK_Places_AspNetUsers_UpdatedById",
                table: "Places");

            migrationBuilder.DropForeignKey(
                name: "FK_States_AspNetUsers_AddedById",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_States_AspNetUsers_UpdatedById",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_Streets_AspNetUsers_AddedById",
                table: "Streets");

            migrationBuilder.DropForeignKey(
                name: "FK_Streets_AspNetUsers_UpdatedById",
                table: "Streets");

            migrationBuilder.DropIndex(
                name: "IX_Streets_AddedById",
                table: "Streets");

            migrationBuilder.DropIndex(
                name: "IX_Streets_UpdatedById",
                table: "Streets");

            migrationBuilder.DropIndex(
                name: "IX_States_AddedById",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_States_UpdatedById",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_Places_AddedById",
                table: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Places_UpdatedById",
                table: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Images_AddedById",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_UpdatedById",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Countries_AddedById",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_UpdatedById",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Cities_AddedById",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Cities_UpdatedById",
                table: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_AddedBYId",
                table: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_UpdatedBYId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "AddedById",
                table: "Streets");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Streets");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Streets");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Streets");

            migrationBuilder.DropColumn(
                name: "AddedById",
                table: "States");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "States");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "States");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "States");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "AddedById",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "AddedById",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "AddedById",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "AddedById",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "AddedBYId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "UpdatedBYId",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "BusinessesId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisitedPlaceId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BusinessesId",
                table: "AspNetUsers",
                column: "BusinessesId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_VisitedPlaceId",
                table: "AspNetUsers",
                column: "VisitedPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Businesses_BusinessesId",
                table: "AspNetUsers",
                column: "BusinessesId",
                principalTable: "Businesses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Places_VisitedPlaceId",
                table: "AspNetUsers",
                column: "VisitedPlaceId",
                principalTable: "Places",
                principalColumn: "Id");
        }
    }
}
