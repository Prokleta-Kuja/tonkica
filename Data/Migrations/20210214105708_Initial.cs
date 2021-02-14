using Microsoft.EntityFrameworkCore.Migrations;

namespace tonkica.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tag = table.Column<string>(type: "TEXT", nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    IsPrefix = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Info = table.Column<string>(type: "TEXT", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ContactInfo = table.Column<string>(type: "TEXT", nullable: false),
                    ContractCurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    ContractRate = table.Column<double>(type: "REAL", nullable: false),
                    DisplayCurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultInvoiceNote = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Currencies_ContractCurrencyId",
                        column: x => x.ContractCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Clients_Currencies_DisplayCurrencyId",
                        column: x => x.DisplayCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Issuers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ContactInfo = table.Column<string>(type: "TEXT", nullable: false),
                    ClockifyUrl = table.Column<string>(type: "TEXT", nullable: true),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issuers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Issuers_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<double>(type: "REAL", nullable: false),
                    Date = table.Column<long>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SequenceNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Subject = table.Column<string>(type: "TEXT", nullable: false),
                    IssuerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayCurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    IssuerCurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayRate = table.Column<double>(type: "REAL", nullable: false),
                    IssuerRate = table.Column<double>(type: "REAL", nullable: false),
                    Total = table.Column<double>(type: "REAL", nullable: false),
                    DisplayTotal = table.Column<double>(type: "REAL", nullable: false),
                    IssuerTotal = table.Column<double>(type: "REAL", nullable: false),
                    QuantityTotal = table.Column<double>(type: "REAL", nullable: false),
                    Published = table.Column<long>(type: "INTEGER", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Currencies_DisplayCurrencyId",
                        column: x => x.DisplayCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Currencies_IssuerCurrencyId",
                        column: x => x.IssuerCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Issuers_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "Issuers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<double>(type: "REAL", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Total = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 1, false, "kn", "HRK" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 2, false, "€", "EUR" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 3, true, "$", "USD" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 4, false, "ł", "GBP" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 5, true, "$", "AUD" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 6, true, "$", "CAD" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 7, false, "Kč", "CZK" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 8, false, "kr.", "DKK" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 9, false, "Ft", "HUF" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 10, true, "¥", "JPY" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 11, false, "kr", "NOK" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 12, false, "kr", "SEK" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 13, false, "francs", "CHF" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 14, false, "KM", "BAM" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "IsPrefix", "Symbol", "Tag" },
                values: new object[] { 15, false, "zł", "PLN" });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CurrencyId",
                table: "Accounts",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ContractCurrencyId",
                table: "Clients",
                column: "ContractCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_DisplayCurrencyId",
                table: "Clients",
                column: "DisplayCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_AccountId",
                table: "Invoices",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ClientId",
                table: "Invoices",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CurrencyId",
                table: "Invoices",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_DisplayCurrencyId",
                table: "Invoices",
                column: "DisplayCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_IssuerCurrencyId",
                table: "Invoices",
                column: "IssuerCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_IssuerId",
                table: "Invoices",
                column: "IssuerId");

            migrationBuilder.CreateIndex(
                name: "IX_Issuers_CurrencyId",
                table: "Issuers",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Issuers");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
