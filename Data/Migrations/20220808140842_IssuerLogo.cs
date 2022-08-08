using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tonkica.Data.Migrations
{
    public partial class IssuerLogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoFileName",
                table: "Issuers",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoFileName",
                table: "Issuers");
        }
    }
}
