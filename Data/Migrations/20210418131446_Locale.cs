using Microsoft.EntityFrameworkCore.Migrations;

namespace tonkica.Data.Migrations
{
    public partial class Locale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Locale",
                table: "Issuers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Issuers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Locale",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Clients",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Locale",
                table: "Issuers");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Issuers");

            migrationBuilder.DropColumn(
                name: "Locale",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Clients");
        }
    }
}
