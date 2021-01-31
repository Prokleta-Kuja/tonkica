using System;
using System.IO;

namespace tonkica
{
    public static class C
    {
        public static class Routes
        {
            public const string Root = "/";
            public const string Currencies = "/currencies";
        }
        public static class Settings
        {
            public static string DataPath(string file) => Path.Combine(Environment.CurrentDirectory, "data", file);
            public static readonly string AppDbConnectionString = $"Data Source={DataPath("app.db")}";
        }
    }
}