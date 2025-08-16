using ExhibitionCurationPlatform.Models;
using System.Text.Json;

namespace ExhibitionCurationPlatform.Mappers
{
    public static class ArtworkMapper
    {
        public static Artwork FromMetMuseumJson(JsonElement json) => new Artwork
        {
            Id = json.TryGetProperty("objectID", out var idProp)
          ? idProp.ToString()
          : Guid.NewGuid().ToString(),

            Title = json.TryGetProperty("title", out var titleProp)
          ? titleProp.GetString()
          : "Untitled",

            Artist = json.TryGetProperty("artistDisplayName", out var artistProp)
          ? string.IsNullOrWhiteSpace(artistProp.GetString()) ? "Unknown" : artistProp.GetString()
          : "Unknown",

            ImageUrl = json.TryGetProperty("primaryImage", out var imageProp)
          ? string.IsNullOrWhiteSpace(imageProp.GetString()) ? null : imageProp.GetString()
          : null,

            Date = json.TryGetProperty("objectDate", out var dateProp)
          ? dateProp.GetString()
          : "Date unknown",

            Source = "Met"
        };

        public static Artwork FromHarvardJson(JsonElement json) => new Artwork
        {
            Id = json.TryGetProperty("id", out var idProp) ? idProp.ToString() : Guid.NewGuid().ToString(),

            Title = json.TryGetProperty("title", out var titleProp)
           ? titleProp.GetString()
           : "Untitled",

            Artist = json.TryGetProperty("people", out var peopleProp) && peopleProp.GetArrayLength() > 0 &&
                peopleProp[0].TryGetProperty("name", out var nameProp)
           ? nameProp.GetString()
           : "Unknown",

            ImageUrl = json.TryGetProperty("primaryimageurl", out var imageProp)
           ? imageProp.GetString()
           : null,

            Date = json.TryGetProperty("dated", out var datedProp)
           ? datedProp.GetString()
           : "Date unknown",

            Source = "Harvard"
        };
    }
}
