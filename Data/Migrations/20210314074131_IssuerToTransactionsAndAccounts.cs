using Microsoft.EntityFrameworkCore.Migrations;

namespace tonkica.Data.Migrations
{
    public partial class IssuerToTransactionsAndAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "IssuerAmount",
                table: "Transactions",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "IssuerRate",
                table: "Transactions",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "IssuerCurrencyId",
                table: "Transactions",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.Sql("UPDATE Transactions SET IssuerCurrencyId = (SELECT CurrencyId FROM Issuers LIMIT 1)");

            migrationBuilder.AlterColumn<int>(
                name: "IssuerCurrencyId",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IssuerId",
                table: "Accounts",
                type: "INTEGER",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.Sql("UPDATE Accounts SET IssuerId = (SELECT Id FROM Issuers LIMIT 1)");

            migrationBuilder.AlterColumn<int>(
                name: "IssuerId",
                table: "Accounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_IssuerCurrencyId",
                table: "Transactions",
                column: "IssuerCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_IssuerId",
                table: "Accounts",
                column: "IssuerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Issuers_IssuerId",
                table: "Accounts",
                column: "IssuerId",
                principalTable: "Issuers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Currencies_IssuerCurrencyId",
                table: "Transactions",
                column: "IssuerCurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Issuers_IssuerId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Currencies_IssuerCurrencyId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_IssuerCurrencyId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_IssuerId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IssuerAmount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "IssuerCurrencyId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "IssuerRate",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "IssuerId",
                table: "Accounts");
        }
    }
}
