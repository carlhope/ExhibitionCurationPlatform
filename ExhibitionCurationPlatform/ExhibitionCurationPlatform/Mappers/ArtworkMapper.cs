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
            Artist = string.IsNullOrWhiteSpace(GetSafeString(json, "artistDisplayName", "")) ? "Unknown"
                   : GetSafeString(json, "artistDisplayName", "Unknown"),
            ImageUrl = GetOptionalString(json, "primaryImage"),
            Date = GetSafeString(json, "objectDate", "Date unknown"),
            Source = "Met"
        };

        public static Artwork FromHarvardJson(JsonElement json) => new Artwork
        {
            Id = GetSafeString(json, "id", Guid.NewGuid().ToString()),
            Title = GetSafeString(json, "title", "Untitled"),
            Artist = json.TryGetProperty("people", out var peopleProp)
                && peopleProp.ValueKind == JsonValueKind.Array
                && peopleProp.GetArrayLength() > 0
                && peopleProp[0].TryGetProperty("name", out var nameProp)
                && nameProp.ValueKind == JsonValueKind.String
                    ? nameProp.GetString() ?? "Unknown"
                    : "Unknown",
            ImageUrl = GetOptionalString(json, "primaryimageurl"),
            Date = GetSafeString(json, "dated", "Date unknown"),
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
    }
}
