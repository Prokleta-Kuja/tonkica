using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace tonkica.Services
{
    public class CurrencyRatesClient
    {
        private readonly ILogger<CurrencyRatesClient> _logger;
        private readonly HttpClient _client;
        public CurrencyRatesClient(ILogger<CurrencyRatesClient> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;

            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.BaseAddress = new Uri("http://api.hnb.hr/tecajn/v2");
        }


        public async Task<IEnumerable<CurrencyRate>> GetLatestRates(IEnumerable<string> currencies, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder("?");
            foreach (var currency in currencies)
                sb.Append($"&valuta={currency}");

            var query = sb.ToString();
            var result = await GetRates(query, currencies, cancellationToken);
            return result;
        }

        public async Task<IEnumerable<CurrencyRate>> GetRatesForDate(int year, int month, int day, IEnumerable<string> currencies, CancellationToken cancellationToken = default)
        {
            var sb = new StringBuilder($"?datum-primjene={year}-{month}-{day}");
            foreach (var currency in currencies)
                sb.Append($"&valuta={currency}");

            var query = sb.ToString();
            var result = await GetRates(query, currencies, cancellationToken);
            return result;
        }
        private async Task<IEnumerable<CurrencyRate>> GetRates(string query, IEnumerable<string> currencies, CancellationToken cancellationToken = default)
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
        public double Unit { get; set; }

        [JsonPropertyName("kupovni_tecaj")]
        public string BuyRateString { get; set; } = null!;
        public double BuyRate => double.Parse(BuyRateString, ci);

        [JsonPropertyName("srednji_tecaj")]
        public string AverageRateString { get; set; } = null!;
        public double AverageRate => double.Parse(AverageRateString, ci);

        [JsonPropertyName("prodajni_tecaj")]
        public string SellRateString { get; set; } = null!;
        public double SellRate => double.Parse(SellRateString, ci);
    }
}