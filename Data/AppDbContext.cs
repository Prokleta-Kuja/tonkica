using System;
using System.Collections.Generic;
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
        public DbSet<Issuer> Issuers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Issuer>(e =>
            {
                e.HasOne(p => p.Currency).WithMany(p => p!.Issuers).HasForeignKey(p => p.CurrencyId);
            });

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

            builder.Entity<Currency>().HasData(
                new Currency("HRK", "kn", false) { Id = 1 },
                new Currency("EUR", "€", false) { Id = 2 },
                new Currency("USD", "$", true) { Id = 3 },
                new Currency("GBP", "ł", false) { Id = 4 },
                new Currency("AUD", "$", true) { Id = 5 },
                new Currency("CAD", "$", true) { Id = 6 },
                new Currency("CZK", "Kč", false) { Id = 7 },
                new Currency("DKK", "kr.", false) { Id = 8 },
                new Currency("HUF", "Ft", false) { Id = 9 },
                new Currency("JPY", "¥", true) { Id = 10 },
                new Currency("NOK", "kr", false) { Id = 11 },
                new Currency("SEK", "kr", false) { Id = 12 },
                new Currency("CHF", "francs", false) { Id = 13 },
                new Currency("BAM", "KM", false) { Id = 14 },
                new Currency("PLN", "zł", false) { Id = 15 }
            );

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

            var client1 = new Client("Klijent", "Ulica Pere Kvržice 3\n10 000 Zagreb\nHrvatska", eur.Id, usd.Id)
            {
                DefaultInvoiceNote = "Plaćanje po ugovoru xxx"
            };
            Clients.AddRange(client1);

            var issuer1 = new Issuer("Moja tvrtka", "Ulica izdavača 7\n10 000 Zagreb\nHrvatska", hrk.Id);
            Issuers.AddRange(issuer1);
            await SaveChangesAsync();

            var invoice1 = new Invoice
            {
                Subject = "Usluge obavljene u siječnju",
                ClientId = client1.Id,
                IssuerId = issuer1.Id,
                CurrencyId = client1.ContractCurrencyId,
                DisplayCurrencyId = client1.DisplayCurrencyId,
                IssuerCurrencyId = issuer1.CurrencyId,
                Note = client1.DefaultInvoiceNote,
                Status = Enums.InvoiceStatus.Draft,
                DispalyRate = 7.556M,
                IssuerRate = 1.3M
            };

            invoice1.Items = new List<InvoiceItem>();
            invoice1.Items.Add(new InvoiceItem("Stavka 1")
            {
                Price = 26,
                Quantity = 140,
                Total = 26 * 140,
            });
            invoice1.Items.Add(new InvoiceItem("Stavka 2")
            {
                Price = 26,
                Quantity = 20,
                Total = 26 * 20,
            });

            // TODO: calculate currencies and totals

            Invoices.Add(invoice1);
            await SaveChangesAsync();
        }
    }
}