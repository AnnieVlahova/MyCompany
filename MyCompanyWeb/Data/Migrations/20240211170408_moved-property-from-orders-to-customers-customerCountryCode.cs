using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCompanyWeb.Data.Migrations
{
    public partial class movedpropertyfromorderstocustomerscustomerCountryCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EIK",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "CustomerCountryCode",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerCountryCode",
                table: "Customer");

            migrationBuilder.AddColumn<int>(
                name: "EIK",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
