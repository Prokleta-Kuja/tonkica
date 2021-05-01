using Microsoft.EntityFrameworkCore.Migrations;

namespace tonkica.Data.Migrations
{
    public partial class RequiredInvoiceElements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IssuedByEmployee",
                table: "Issuers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DueInDays",
                table: "Clients",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuedByEmployee",
                table: "Issuers");

            migrationBuilder.DropColumn(
                name: "DueInDays",
                table: "Clients");
        }
    }
}
