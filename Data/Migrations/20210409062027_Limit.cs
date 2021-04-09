using Microsoft.EntityFrameworkCore.Migrations;

namespace tonkica.Data.Migrations
{
    public partial class Limit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Limit",
                table: "Issuers",
                type: "REAL",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Limit",
                table: "Issuers");
        }
    }
}
