using ExhibitionCurationPlatform.Mappers;
using ExhibitionCurationPlatform.Models;
using ExhibitionCurationPlatform.Services.Interfaces;
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

        public async Task<List<Artwork>> SearchAsync(string query)
        {
            var searchUrl = $"https://collectionapi.metmuseum.org/public/collection/v1/search?q={Uri.EscapeDataString(query)}";
            var searchResponse = await _http.GetFromJsonAsync<JsonElement>(searchUrl);

            if (!searchResponse.TryGetProperty("objectIDs", out var ids) || ids.GetArrayLength() == 0)
                return [];

            var artworks = new List<Artwork>();
            foreach (var id in ids.EnumerateArray().Take(10))
            {
                var objUrl = $"https://collectionapi.metmuseum.org/public/collection/v1/objects/{id}";
                var objResponse = await _http.GetFromJsonAsync<JsonElement>(objUrl);

                artworks.Add(ArtworkMapper.FromMetMuseumJson(objResponse));
            }

            return artworks;
        }
    }
}
