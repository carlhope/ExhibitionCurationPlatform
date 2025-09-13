using ExhibitionCurationPlatform.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

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
            if (string.IsNullOrWhiteSpace(rawDate))
                return fallback;

            rawDate = rawDate.ToLower().Trim();

            // Try full date
            if (DateOnly.TryParse(rawDate, out var parsedFull))
                return parsedFull;

            // Try year-only
            if (int.TryParse(rawDate, out var year) && year > 0)
                return new DateOnly(year, 1, 1);

            // Strip prefixes like "ca.", "c.", "late", "early", "mid"
            rawDate = Regex.Replace(rawDate, @"\b(ca\.|c\.|late|early|mid|–|to)\b", "", RegexOptions.IgnoreCase).Trim();

            // Handle century phrases
            var centuryMatch = Regex.Match(rawDate, @"(\d+)(?:st|nd|rd|th)? century");
            if (centuryMatch.Success && int.TryParse(centuryMatch.Groups[1].Value, out var century))
                return new DateOnly((century - 1) * 100 + 1, 1, 1);

            // Handle ranges like "19th–20th century"
            var rangeMatch = Regex.Match(rawDate, @"(\d+)(?:st|nd|rd|th)?\s*[-–]\s*(\d+)(?:st|nd|rd|th)? century");
            if (rangeMatch.Success && int.TryParse(rangeMatch.Groups[1].Value, out var startCentury))
                return new DateOnly((startCentury - 1) * 100 + 1, 1, 1);

            // Handle hybrid phrases like "late 19th–early 20th century"
            var hybridMatch = Regex.Match(rawDate, @"(\d+)(?:st|nd|rd|th)? century");
            if (hybridMatch.Success && int.TryParse(hybridMatch.Groups[1].Value, out var hybridCentury))
                return new DateOnly((hybridCentury - 1) * 100 + 1, 1, 1);

            return fallback;
        }
    }
}
