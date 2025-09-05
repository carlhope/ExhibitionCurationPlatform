using ExhibitionCurationPlatform.Models;
using System.Text.Json;

namespace ExhibitionCurationPlatform.Mappers
{
    public static class ArtworkMapper
    {
        private static string GetSafeString(JsonElement json, string propertyName, string fallback)
        {
            return json.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String
                ? prop.GetString() ?? fallback
                : fallback;
        }

        private static string? GetOptionalString(JsonElement json, string propertyName)
        {
            return json.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String
                ? string.IsNullOrWhiteSpace(prop.GetString()) ? null : prop.GetString()
                : null;
        }

        public static Artwork FromMetMuseumJson(JsonElement json) => new Artwork
        {
            Id = GetFlexibleId(json, "objectID"),
            Title = GetSafeString(json, "title", "Untitled"),
            Description = ComposeMetDescription(json),
            Artist = GetSafeString(json, "artistDisplayName", "Unknown"),
            ImageUrl = GetOptionalString(json, "primaryImage"),
            Date = ParseDateOrDefault(GetSafeString(json, "objectDate", ""), new DateOnly(1, 1, 1)),
            DateAsString = GetSafeString(json, "objectDate", "Date unknown"),
            Source = "Met"
        };

        public static Artwork FromHarvardJson(JsonElement json) => new Artwork
        {
            Id = GetFlexibleId(json, "id"),
            Title = GetSafeString(json, "title", "Untitled"),
            Description = ComposeHarvardDescription(json),
            Artist = GetHarvardArtist(json),
            ImageUrl = GetOptionalString(json, "primaryimageurl"),
            Date = ParseDateOrDefault(GetSafeString(json, "dated", ""), new DateOnly(1, 1, 1)),
            DateAsString = GetSafeString(json, "dated", "Date unknown"),
            Source = "Harvard"
        };

        private static string GetFlexibleId(JsonElement json, string propertyName)
        {
            if (!json.TryGetProperty(propertyName, out var prop)) return Guid.NewGuid().ToString();

            return prop.ValueKind switch
            {
                JsonValueKind.String => prop.GetString() ?? Guid.NewGuid().ToString(),
                JsonValueKind.Number => prop.TryGetInt32(out var intId) ? intId.ToString() : Guid.NewGuid().ToString(),
                _ => Guid.NewGuid().ToString()
            };
        }
        private static string GetHarvardArtist(JsonElement json)
        {
            if (json.TryGetProperty("people", out var peopleProp)
                && peopleProp.ValueKind == JsonValueKind.Array
                && peopleProp.GetArrayLength() > 0)
            {
                var firstPerson = peopleProp[0];
                if (firstPerson.TryGetProperty("name", out var nameProp)
                    && nameProp.ValueKind == JsonValueKind.String)
                {
                    return nameProp.GetString() ?? "Unknown";
                }
            }
            return "Unknown";
        }
        public static string ComposeMetDescription(JsonElement json)
        {
            var title = GetSafeString(json, "title", "Untitled");
            var artist = GetSafeString(json, "artistDisplayName", "Unknown artist");
            var date = GetSafeString(json, "objectDate", "date unknown");
            return $"“{title}” by {artist}, created in {date}. Part of the Met collection.";
        }
        public static string ComposeHarvardDescription(JsonElement json)
        {
            var title = GetSafeString(json, "title", "Untitled");
            var date = GetSafeString(json, "dated", "date unknown");
            var artist = GetHarvardArtist(json); // already handles fallback
            return $"“{title}” by {artist}, created in {date}. Part of the Harvard collection.";
        }
        public static DateOnly ParseDateOrDefault(string rawDate, DateOnly fallback)
        {
            // Try full date first (e.g. "1889-01-01")
            if (DateOnly.TryParse(rawDate, out var parsedFull))
                return parsedFull;

            // Try year-only (e.g. "1889")
            if (int.TryParse(rawDate, out var year) && year > 0)
                return new DateOnly(year, 1, 1); // Default to Jan 1st of that year

            // Fallback
            return fallback;
        }
    }
}
