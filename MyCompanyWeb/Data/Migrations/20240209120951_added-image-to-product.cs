using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCompanyWeb.Data.Migrations
{
    public partial class addedimagetoproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Product");
        }
    }
}
