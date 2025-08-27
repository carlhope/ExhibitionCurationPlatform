using ExhibitionCurationPlatform.Config;
using ExhibitionCurationPlatform.Mappers;
using ExhibitionCurationPlatform.Models;
using ExhibitionCurationPlatform.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text.Json;

namespace ExhibitionCurationPlatform.Services
{
    public class HarvardArtService : IHarvardArtService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public HarvardArtService(HttpClient http, IOptions<HarvardArtOptions> options)
        {
            _http = http;
            _apiKey = options.Value.ApiKey;
        }

        public async Task<List<Artwork>> SearchAsync(string query, string? filterBy)
        {
            var url = $"https://api.harvardartmuseums.org/object?apikey={_apiKey}&title={Uri.EscapeDataString(query)}&size=10";
            if (!string.IsNullOrEmpty(filterBy))
                url += $"&classification={filterBy}"; // or whatever field the API supports


            var response = await _http.GetFromJsonAsync<JsonElement>(url);

            if (!response.TryGetProperty("records", out var records))
                return [];

            var artworks = new List<Artwork>();
            foreach (var record in records.EnumerateArray())
            {
                artworks.Add(ArtworkMapper.FromHarvardJson(record));
            }

            return artworks;
        }
    }
}
