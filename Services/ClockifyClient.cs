using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace tonkica.Services
{
    public class ClockifyClient
    {
        private const string BASE_ADDRESS = "https://reports.api.clockify.me/v1/shared-reports/";
        private readonly ILogger<ClockifyClient> _logger;
        private readonly HttpClient _client;
        public ClockifyClient(ILogger<ClockifyClient> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;

            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.BaseAddress = new Uri(BASE_ADDRESS);
        }

        public async Task<List<ClockifyEntry>> GetDefaultTimeEntries(string url, DateTime start, DateTime end, CancellationToken cancellationToken = default)
        {
            if (start.Kind != DateTimeKind.Utc || end.Kind != DateTimeKind.Utc)
                throw new FormatException("Dates should be of UTC kind");

            var token = ExtractToken(url);
            var s = start.ToString("o", CultureInfo.InvariantCulture);
            var e = end.ToString("o", CultureInfo.InvariantCulture);
            var endpoint = $"{token}?exportType=JSON&pageSize=9999&dateRangeStart={s}&dateRangeEnd={e}";

            var response = await _client.GetAsync(endpoint, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var root = JsonSerializer.Deserialize<ClockifyResponse>(json);

            if (root?.Entries != null)
                return root.Entries;

            return new List<ClockifyEntry>(0);
        }
        private string ExtractToken(string url)
        {
            var trimmed = url.TrimEnd('/');
            var parts = trimmed.Split('/');

            return parts.Last();
        }

        public class ClockifyInterval
        {

            [JsonPropertyName("duration")]
            public int Duration { get; set; }
        }
        public class ClockifyEntry
        {
            [JsonPropertyName("description")]
            public string Description { get; set; } = null!;
            [JsonPropertyName("client_name")]
            public string ClientName { get; set; } = null!;
            [JsonPropertyName("project_name")]
            public string ProjectName { get; set; } = null!;
            [JsonPropertyName("timeInterval")]
            public ClockifyInterval Interval { get; set; } = null!;
        }

        public class ClockifyResponse
        {
            [JsonPropertyName("timeentries")]
            public List<ClockifyEntry>? Entries { get; set; }
        }
    }
}