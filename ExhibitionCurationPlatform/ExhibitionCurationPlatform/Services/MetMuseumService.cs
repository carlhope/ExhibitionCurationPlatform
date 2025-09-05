using ExhibitionCurationPlatform.Mappers;
using ExhibitionCurationPlatform.Models;
using ExhibitionCurationPlatform.Services.Interfaces;
using System;
using System.Text.Json;

namespace ExhibitionCurationPlatform.Services
{
    public class MetMuseumService : IMetMuseumService
    {
        private readonly HttpClient _http;

        public MetMuseumService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Artwork?> GetByIdAsync(string id)
        {
            var response = await _http.GetAsync($"https://collectionapi.metmuseum.org/public/collection/v1/objects/{id}");
            if (!response.IsSuccessStatusCode) return null;

            using var stream = await response.Content.ReadAsStreamAsync();
            using var json = await JsonDocument.ParseAsync(stream);

            return ArtworkMapper.FromMetMuseumJson(json.RootElement);


        }

        public async Task<List<Artwork>> SearchAsync(string query, string? filterBy)
        {
            var artworks = new List<Artwork>();

            try
            {
                var searchUrl = $"https://collectionapi.metmuseum.org/public/collection/v1/search?q={Uri.EscapeDataString(query)}";
                if (!string.IsNullOrEmpty(filterBy))
                    searchUrl += $"&classification={filterBy}";

                var searchResponse = await _http.GetFromJsonAsync<JsonElement>(searchUrl);

                if (!searchResponse.TryGetProperty("objectIDs", out var ids) || ids.GetArrayLength() == 0)
                    return [];

                var objectIds = ids.EnumerateArray()
                                   .Take(10)
                                   .Select(id => id.GetInt32())
                                   .ToList();

                var fetchTasks = objectIds.Select(id => GetByIdAsync(id.ToString()));


                var results = await Task.WhenAll(fetchTasks);
                artworks = results.Where(a => a != null).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SearchAsync failed: {ex.Message}");
            }

            return artworks;
        }
    }
}
