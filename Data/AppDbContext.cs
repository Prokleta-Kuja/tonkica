using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace tonkica.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Currency> Currencies { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<InvoiceItem> InvoiceItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Client>(e =>
            {
                e.HasOne(p => p.ContractCurrency).WithMany(p => p!.ContractClients).HasForeignKey(p => p.ContractCurrencyId);
                e.HasOne(p => p.DisplayCurrency).WithMany(p => p!.DisplayClients).HasForeignKey(p => p.DisplayCurrencyId);
            });

            builder.Entity<Invoice>(e =>
            {
                e.HasOne(p => p.Client).WithMany(p => p!.Invoices).HasForeignKey(p => p.ClientId);
                e.HasOne(p => p.Currency).WithMany(p => p!.Invoices).HasForeignKey(p => p.CurrencyId);
                e.HasOne(p => p.DisplayCurrency).WithMany(p => p!.DisplayInvoices).HasForeignKey(p => p.DisplayCurrencyId);
                e.HasOne(p => p.IssuerCurrency).WithMany(p => p!.IssuerInvoices).HasForeignKey(p => p.IssuerCurrencyId);
                e.HasMany(p => p.Items).WithOne(p => p!.Invoice!).HasForeignKey(p => p.InvoiceId);
            });


            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var dtProperties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(DateTimeOffset) || p.PropertyType == typeof(DateTimeOffset?));

                foreach (var property in dtProperties)
                    builder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());

                var decProperties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal) || p.PropertyType == typeof(decimal?));

                foreach (var property in decProperties)
                    builder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion<double>();
            }
        }

        public async Task ProvisionDemoAsync()
        {
            var hrk = new Currency("HRK", "kn", false);
            var eur = new Currency("EUR", "€", false);
            var usd = new Currency("USD", "$", true);
            var gbp = new Currency("GBP", "ł", false);
            var aud = new Currency("AUD", "$", true);
            var cad = new Currency("CAD", "$", true);
            var czk = new Currency("CZK", "Kč", false);
            var dkk = new Currency("DKK", "kr.", false);
            var huf = new Currency("HUF", "Ft", false);
            var jpy = new Currency("JPY", "¥", true);
            var nok = new Currency("NOK", "kr", false);
            var sek = new Currency("SEK", "kr", false);
            var chf = new Currency("CHF", "francs", false);
            var bam = new Currency("BAM", "KM", false);
            var pln = new Currency("PLN", "zł", false);

            Currencies.AddRange(hrk, eur, usd, gbp, aud, cad, czk, dkk, huf, jpy, nok, sek, chf, bam, pln);
            await SaveChangesAsync();

            var client1 = new Client("Klijent", "Klijent d.o.o.\nUlica Pere Kvržice 3\n10 000 Zagreb", eur.Id, usd.Id);
            Clients.AddRange(client1);
            await SaveChangesAsync();
        }
    }
}