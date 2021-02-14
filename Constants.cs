using System;
using System.IO;

namespace tonkica
{
    public static class C
    {
        public static class Routes
        {
            public const string Root = "/";
            public const string Accounts = "/accounts";
            public const string Clients = "/clients";
            public const string Invoices = "/invoices";
            public const string Invoice = "/invoices/{Id:int}";
            public static string InvoiceFor(int id) => $"{Invoices}/{id}";
            public const string InvoicePrint = "/invoices/{Id:int}/print";
            public static string InvoicePrintFor(int id) => $"{Invoices}/{id}/print";
            public const string Issuers = "/issuers";
            public const string Transactions = "/transactions";
        }
        public static class Settings
        {
            public static string DataPath(string file) => Path.Combine(Environment.CurrentDirectory, "data", file);
            public static readonly string AppDbConnectionString = $"Data Source={DataPath("app.db")}";
        }
        public static class Formats
        {
            public static string AsDecimal(decimal d, int places = 4) => d.ToString($"#,##0.{new string('0', places)}");
        }
    }
}