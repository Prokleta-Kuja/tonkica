using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using tonkica.Data;

namespace tonkica
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await InitializeDb(args);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        static async Task InitializeDb(string[] args)
        {
            var dbFile = new FileInfo(C.Settings.DataPath("app.db"));
            dbFile.Directory?.Create();

            var opt = new DbContextOptionsBuilder<AppDbContext>();
            opt.UseSqlite(C.Settings.AppDbConnectionString);

            var db = new AppDbContext(opt.Options);
            if (db.Database.GetMigrations().Any())
                await db.Database.MigrateAsync();
            else
                await db.Database.EnsureCreatedAsync();

            // Seed
            if (Debugger.IsAttached && !db.Accounts.Any())
            {
                await db.ProvisionDemoAsync();
            }
        }
    }
}
