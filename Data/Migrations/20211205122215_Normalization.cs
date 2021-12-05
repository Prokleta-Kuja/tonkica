using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tonkica.Data.Migrations
{
    public partial class Normalization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoteNormalized",
                table: "Transactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameNormalized",
                table: "TransactionCategories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameNormalized",
                table: "Issuers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameNormalized",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameNormalized",
                table: "Accounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
            UPDATE Accounts SET NameNormalized = upper(Name) WHERE NameNormalized = '';
            UPDATE Clients SET NameNormalized = upper(Name) WHERE NameNormalized = '';
            UPDATE Issuers SET NameNormalized = upper(Name) WHERE NameNormalized = '';
            UPDATE TransactionCategories SET NameNormalized = upper(Name) WHERE NameNormalized = '';
            UPDATE Transactions SET NoteNormalized = upper(Note) WHERE NoteNormalized IS NULL OR NoteNormalized = '';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoteNormalized",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "NameNormalized",
                table: "TransactionCategories");

            migrationBuilder.DropColumn(
                name: "NameNormalized",
                table: "Issuers");

            migrationBuilder.DropColumn(
                name: "NameNormalized",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "NameNormalized",
                table: "Accounts");
        }
    }
}
