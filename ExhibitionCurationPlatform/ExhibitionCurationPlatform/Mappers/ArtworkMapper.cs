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
            Artist = GetSafeString(json, "artistDisplayName", "Unknown"),
            ImageUrl = GetOptionalString(json, "primaryImage"),
            Date = GetSafeString(json, "objectDate", "Date unknown"),
            Source = "Met"
        };

        public static Artwork FromHarvardJson(JsonElement json) => new Artwork
        {
            Id = GetFlexibleId(json, "id"),
            Title = GetSafeString(json, "title", "Untitled"),
            Artist = GetHarvardArtist(json),
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
    }
}
