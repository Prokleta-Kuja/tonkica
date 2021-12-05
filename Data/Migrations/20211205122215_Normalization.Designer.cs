﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using tonkica.Data;

#nullable disable

namespace tonkica.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20211205122215_Normalization")]
    partial class Normalization
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.DataProtectionKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FriendlyName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Xml")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DataProtectionKeys");
                });

            modelBuilder.Entity("tonkica.Data.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Info")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("IssuerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameNormalized")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("IssuerId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("tonkica.Data.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ContactInfo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ContractCurrencyId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("ContractRate")
                        .HasColumnType("REAL");

                    b.Property<string>("DefaultInvoiceNote")
                        .HasColumnType("TEXT");

                    b.Property<int>("DisplayCurrencyId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("DueInDays")
                        .HasColumnType("REAL");

                    b.Property<string>("Locale")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameNormalized")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ContractCurrencyId");

                    b.HasIndex("DisplayCurrencyId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("tonkica.Data.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPrefix")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Currencies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsPrefix = false,
                            Symbol = "kn",
                            Tag = "HRK"
                        },
                        new
                        {
                            Id = 2,
                            IsPrefix = false,
                            Symbol = "€",
                            Tag = "EUR"
                        },
                        new
                        {
                            Id = 3,
                            IsPrefix = true,
                            Symbol = "$",
                            Tag = "USD"
                        },
                        new
                        {
                            Id = 4,
                            IsPrefix = false,
                            Symbol = "ł",
                            Tag = "GBP"
                        },
                        new
                        {
                            Id = 5,
                            IsPrefix = true,
                            Symbol = "$",
                            Tag = "AUD"
                        },
                        new
                        {
                            Id = 6,
                            IsPrefix = true,
                            Symbol = "$",
                            Tag = "CAD"
                        },
                        new
                        {
                            Id = 7,
                            IsPrefix = false,
                            Symbol = "Kč",
                            Tag = "CZK"
                        },
                        new
                        {
                            Id = 8,
                            IsPrefix = false,
                            Symbol = "kr.",
                            Tag = "DKK"
                        },
                        new
                        {
                            Id = 9,
                            IsPrefix = false,
                            Symbol = "Ft",
                            Tag = "HUF"
                        },
                        new
                        {
                            Id = 10,
                            IsPrefix = true,
                            Symbol = "¥",
                            Tag = "JPY"
                        },
                        new
                        {
                            Id = 11,
                            IsPrefix = false,
                            Symbol = "kr",
                            Tag = "NOK"
                        },
                        new
                        {
                            Id = 12,
                            IsPrefix = false,
                            Symbol = "kr",
                            Tag = "SEK"
                        },
                        new
                        {
                            Id = 13,
                            IsPrefix = false,
                            Symbol = "francs",
                            Tag = "CHF"
                        },
                        new
                        {
                            Id = 14,
                            IsPrefix = false,
                            Symbol = "KM",
                            Tag = "BAM"
                        },
                        new
                        {
                            Id = 15,
                            IsPrefix = false,
                            Symbol = "zł",
                            Tag = "PLN"
                        });
                });

            modelBuilder.Entity("tonkica.Data.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DisplayCurrencyId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("DisplayRate")
                        .HasColumnType("REAL");

                    b.Property<double>("DisplayTotal")
                        .HasColumnType("REAL");

                    b.Property<int>("IssuerCurrencyId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IssuerId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("IssuerRate")
                        .HasColumnType("REAL");

                    b.Property<double>("IssuerTotal")
                        .HasColumnType("REAL");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<long?>("Published")
                        .HasColumnType("INTEGER");

                    b.Property<double>("QuantityTotal")
                        .HasColumnType("REAL");

                    b.Property<string>("SequenceNumber")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Total")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ClientId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("DisplayCurrencyId");

                    b.HasIndex("IssuerCurrencyId");

                    b.HasIndex("IssuerId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("tonkica.Data.InvoiceItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("InvoiceId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<double>("Quantity")
                        .HasColumnType("REAL");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Total")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.ToTable("InvoiceItems");
                });

            modelBuilder.Entity("tonkica.Data.Issuer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClockifyUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactInfo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IssuedByEmployee")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double?>("Limit")
                        .HasColumnType("REAL");

                    b.Property<string>("Locale")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameNormalized")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Issuers");
                });

            modelBuilder.Entity("tonkica.Data.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Date")
                        .HasColumnType("INTEGER");

                    b.Property<double>("IssuerAmount")
                        .HasColumnType("REAL");

                    b.Property<int>("IssuerCurrencyId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("IssuerRate")
                        .HasColumnType("REAL");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<string>("NoteNormalized")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("IssuerCurrencyId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("tonkica.Data.TransactionCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameNormalized")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TransactionCategories");
                });

            modelBuilder.Entity("tonkica.Data.Account", b =>
                {
                    b.HasOne("tonkica.Data.Currency", "Currency")
                        .WithMany("Accounts")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("tonkica.Data.Issuer", "Issuer")
                        .WithMany("Accounts")
                        .HasForeignKey("IssuerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Issuer");
                });

            modelBuilder.Entity("tonkica.Data.Client", b =>
                {
                    b.HasOne("tonkica.Data.Currency", "ContractCurrency")
                        .WithMany("ContractClients")
                        .HasForeignKey("ContractCurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("tonkica.Data.Currency", "DisplayCurrency")
                        .WithMany("DisplayClients")
                        .HasForeignKey("DisplayCurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContractCurrency");

                    b.Navigation("DisplayCurrency");
                });

            modelBuilder.Entity("tonkica.Data.Invoice", b =>
                {
                    b.HasOne("tonkica.Data.Account", "Account")
                        .WithMany("Invoices")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("tonkica.Data.Client", "Client")
                        .WithMany("Invoices")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("tonkica.Data.Currency", "Currency")
                        .WithMany("Invoices")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("tonkica.Data.Currency", "DisplayCurrency")
                        .WithMany("DisplayInvoices")
                        .HasForeignKey("DisplayCurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("tonkica.Data.Currency", "IssuerCurrency")
                        .WithMany("IssuerInvoices")
                        .HasForeignKey("IssuerCurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("tonkica.Data.Issuer", "Issuer")
                        .WithMany("Invoices")
                        .HasForeignKey("IssuerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Client");

                    b.Navigation("Currency");

                    b.Navigation("DisplayCurrency");

                    b.Navigation("Issuer");

                    b.Navigation("IssuerCurrency");
                });

            modelBuilder.Entity("tonkica.Data.InvoiceItem", b =>
                {
                    b.HasOne("tonkica.Data.Invoice", "Invoice")
                        .WithMany("Items")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("tonkica.Data.Issuer", b =>
                {
                    b.HasOne("tonkica.Data.Currency", "Currency")
                        .WithMany("Issuers")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("tonkica.Data.Transaction", b =>
                {
                    b.HasOne("tonkica.Data.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("tonkica.Data.TransactionCategory", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("tonkica.Data.Currency", "IssuerCurrency")
                        .WithMany("Transactions")
                        .HasForeignKey("IssuerCurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Category");

                    b.Navigation("IssuerCurrency");
                });

            modelBuilder.Entity("tonkica.Data.Account", b =>
                {
                    b.Navigation("Invoices");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("tonkica.Data.Client", b =>
                {
                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("tonkica.Data.Currency", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("ContractClients");

                    b.Navigation("DisplayClients");

                    b.Navigation("DisplayInvoices");

                    b.Navigation("Invoices");

                    b.Navigation("IssuerInvoices");

                    b.Navigation("Issuers");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("tonkica.Data.Invoice", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("tonkica.Data.Issuer", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Invoices");
                });

            modelBuilder.Entity("tonkica.Data.TransactionCategory", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
