using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelManagementSystem.Migrations
{
    public partial class AddedadminverifyStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminVerificationStatus",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminVerificationStatus",
                table: "Places");
        }
    }
}
