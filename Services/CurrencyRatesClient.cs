using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using tonkica.Data;

namespace tonkica.Services
{
    public class CurrencyRatesClient
    {
        private readonly ILogger<CurrencyRatesClient> _logger;
        private readonly HttpClient _client;
        public const string BASE_CURRENCY = "HRK";
        public CurrencyRatesClient(ILogger<CurrencyRatesClient> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;

            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.BaseAddress = new Uri("http://api.hnb.hr/tecajn/v2");
        }

        public async Task CalculateRates(Transaction t)
        {
            if (t.Account?.Currency == null || t.Account?.Issuer?.Currency == null)
                throw new ArgumentNullException(nameof(Currency));

            var tags = new HashSet<string> { t.Account.Currency.Tag, t.Account.Issuer.Currency.Tag, };

            if (tags.Count == 1)
            {
                t.IssuerRate = 1;
                return;
            }

            var rates = await GetRatesForDateAsync(t.Date, tags);

            var accountRate = rates.FirstOrDefault(x => x.Currency == t.Account.Currency.Tag);
            var issuerRate = rates.FirstOrDefault(x => x.Currency == t.Account.Issuer.Currency.Tag);


            if (issuerRate == null && accountRate != null) // Issuer is in HRK and Transaction/Account is not HRK
                t.IssuerRate = accountRate.AverageRate / accountRate.Unit;
            else if (accountRate == null && issuerRate != null) // Transaction/Account is in HRK and Issuer is not HRK
                t.IssuerRate = 1 / (issuerRate.AverageRate / issuerRate.Unit);
            else // Nothing is in HRK
                t.IssuerRate = (accountRate!.AverageRate / accountRate.Unit) / (issuerRate!.AverageRate / issuerRate.Unit);

        }

        public async Task CalculateRates(Invoice i)
        {
            if (i.Currency == null || i.DisplayCurrency == null || i.IssuerCurrency == null)
                throw new ArgumentNullException(nameof(Currency));

            var tags = new HashSet<string> { i.Currency.Tag, i.DisplayCurrency.Tag, i.IssuerCurrency.Tag, };

            // Faking HNB API v2 je krepao...
            // var rates = i.Published.HasValue
            //     ? await GetRatesForDateAsync(i.Published.Value, tags)
            //     : await GetLatestRatesAsync(tags);

            // var contractRate = rates.FirstOrDefault(x => x.Currency == i.Currency.Tag);
            // var displayRate = rates.FirstOrDefault(x => x.Currency == i.DisplayCurrency.Tag);
            // var issuerRate = rates.FirstOrDefault(x => x.Currency == i.IssuerCurrency.Tag);

            // if (contractRate == null) // Contract is in HRK
            // {
            //     i.DisplayRate = displayRate == null ? 1 : displayRate.AverageRate / displayRate.Unit;
            //     i.IssuerRate = issuerRate == null ? 1 : issuerRate.AverageRate / issuerRate.Unit;
            // }
            // else
            // {
            //     if (displayRate == null)
            //         i.DisplayRate = contractRate.AverageRate / contractRate.Unit;
            //     else if (displayRate.Currency == contractRate.Currency)
            //         i.DisplayRate = 1;
            //     else
            //         i.DisplayRate = (contractRate.AverageRate / contractRate.Unit) / (displayRate.AverageRate / displayRate.Unit);

            //     if (issuerRate == null)
            //         i.IssuerRate = contractRate.AverageRate / contractRate.Unit;
            //     else if (issuerRate.Currency == contractRate.Currency)
            //         i.IssuerRate = 1;
            //     else
            //         i.IssuerRate = (contractRate.AverageRate / contractRate.Unit) / (issuerRate.AverageRate / issuerRate.Unit);
            // }

            i.DisplayRate = 1;
            i.IssuerRate = 1;
        }
        public Task<IEnumerable<CurrencyRate>> GetLatestRatesAsync(IEnumerable<string> currencies, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder("?");
            foreach (var currency in currencies)
                sb.Append($"&valuta={currency}");

            var query = sb.ToString();
            return GetRatesAsync(query, currencies, cancellationToken);
        }

        public Task<IEnumerable<CurrencyRate>> GetRatesForDateAsync(DateTimeOffset dt, IEnumerable<string> currencies, CancellationToken cancellationToken = default)
            => GetRatesForDateAsync(dt.Year, dt.Month, dt.Day, currencies, cancellationToken);
        public Task<IEnumerable<CurrencyRate>> GetRatesForDateAsync(DateTime dt, IEnumerable<string> currencies, CancellationToken cancellationToken = default)
            => GetRatesForDateAsync(dt.Year, dt.Month, dt.Day, currencies, cancellationToken);

        public Task<IEnumerable<CurrencyRate>> GetRatesForDateAsync(int year, int month, int day, IEnumerable<string> currencies, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder($"?datum-primjene={year}-{month}-{day}");
            foreach (var currency in currencies)
                sb.Append($"&valuta={currency}");

            var query = sb.ToString();
            return GetRatesAsync(query, currencies, cancellationToken);
        }
        public Task<IEnumerable<CurrencyRate>> GetRatesForPeriodAsync(DateTimeOffset from, DateTimeOffset to, IEnumerable<string> currencies, CancellationToken cancellationToken = default)
            => GetRatesForPeriodAsync(from.UtcDateTime, to.UtcDateTime, currencies, cancellationToken);
        public Task<IEnumerable<CurrencyRate>> GetRatesForPeriodAsync(DateTime from, DateTime to, IEnumerable<string> currencies, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder($"?datum-primjene-od={from.Year}-{from.Month}-{from.Day}&datum-primjene-do={to.Year}-{to.Month}-{to.Day}");
            foreach (var currency in currencies)
                sb.Append($"&valuta={currency}");

            var query = sb.ToString();
            return GetRatesAsync(query, currencies, cancellationToken);
        }
        private async Task<IEnumerable<CurrencyRate>> GetRatesAsync(string query, IEnumerable<string> currencies, CancellationToken cancellationToken = default)
        {
            var response = await _client.GetAsync(query, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<IEnumerable<CurrencyRate>>(json) ?? new List<CurrencyRate>();
            return result;
        }
    }
    public class CurrencyRate
    {
        private static readonly CultureInfo ci = new CultureInfo("HR-hr");

        [JsonPropertyName("datum_primjene")]
        public DateTimeOffset ForDate { get; set; }

        [JsonPropertyName("valuta")]
        public string Currency { get; set; } = null!;

        [JsonPropertyName("jedinica")]
        public decimal Unit { get; set; }

        [JsonPropertyName("kupovni_tecaj")]
        public string BuyRateString { get; set; } = null!;
        public decimal BuyRate => decimal.Parse(BuyRateString, ci);

        [JsonPropertyName("srednji_tecaj")]
        public string AverageRateString { get; set; } = null!;
        public decimal AverageRate => decimal.Parse(AverageRateString, ci);

        [JsonPropertyName("prodajni_tecaj")]
        public string SellRateString { get; set; } = null!;
        public decimal SellRate => decimal.Parse(SellRateString, ci);
    }
}
