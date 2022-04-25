using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelManagementSystem.Migrations
{
    public partial class AddedVisitedcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsVisited",
                table: "Places",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdminVerified",
                table: "Places",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdminVerified",
                table: "Places");

            migrationBuilder.AlterColumn<string>(
                name: "IsVisited",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
