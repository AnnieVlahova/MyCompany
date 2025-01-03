using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCompanyWeb.Data.Migrations
{
    public partial class addedsubtotalandfinalPricefororders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FinalPrice",
                table: "OrderProduct",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Subtotal",
                table: "Order",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "Subtotal",
                table: "Order");
        }
    }
}
