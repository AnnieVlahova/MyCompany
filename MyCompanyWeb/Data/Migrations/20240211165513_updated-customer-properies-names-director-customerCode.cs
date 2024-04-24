using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyCompanyWeb.Data.Migrations
{
    public partial class updatedcustomerproperiesnamesdirectorcustomerCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NeispuoCode",
                table: "Customer",
                newName: "CustomerCode");

            migrationBuilder.RenameColumn(
                name: "MolDirector",
                table: "Customer",
                newName: "Director");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Director",
                table: "Customer",
                newName: "MolDirector");

            migrationBuilder.RenameColumn(
                name: "CustomerCode",
                table: "Customer",
                newName: "NeispuoCode");
        }
    }
}
