using System;
using System.IO;

namespace tonkica
{
    public static class C
    {
        public static class Routes
        {
            public const string Root = "/";
            public const string Invoices = "/invoices";
            public const string Invoice = "/invoices/{Id:int}";
            public static string InvoiceFor(int id) => $"{Invoices}/{id}";
            public const string Clients = "/clients";
            public const string Issuers = "/issuers";
        }
        public static class Settings
        {
            public static string DataPath(string file) => Path.Combine(Environment.CurrentDirectory, "data", file);
            public static readonly string AppDbConnectionString = $"Data Source={DataPath("app.db")}";
        }
    }
}