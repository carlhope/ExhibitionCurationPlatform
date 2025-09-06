using ExhibitionCurationPlatform.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExhibitionCurationPlatform.Tests
{
    public class MetMapperTests
    {
        [Fact]
        public void FromMetMuseumJson_MapsAllFieldsCorrectly()
        {
            var json = JsonDocument.Parse("""
    {
        "objectID": 12345,
        "title": "Sunflowers",
        "artistDisplayName": "Vincent van Gogh",
        "primaryImage": "http://example.com/image.jpg",
        "objectDate": "1888"
    }
    """).RootElement;

            var result = ArtworkMapper.FromMetMuseumJson(json);

            Assert.Equal("12345", result.Id);
            Assert.Equal("Sunflowers", result.Title);
            Assert.Equal("Vincent van Gogh", result.Artist);
            Assert.Equal("http://example.com/image.jpg", result.ImageUrl);
            Assert.Equal("1888", result.DateAsString);
            Assert.Equal("Met", result.Source);
        }

        [Fact]
        public void FromMetMuseumJson_HandlesMissingFieldsGracefully()
        {
            var json = JsonDocument.Parse("{}").RootElement;

            var result = ArtworkMapper.FromMetMuseumJson(json);

            Assert.False(string.IsNullOrWhiteSpace(result.Id)); // Should be a GUID
            Assert.Equal("Untitled", result.Title);
            Assert.Equal("Unknown", result.Artist);
            Assert.Null(result.ImageUrl);
            Assert.Equal("Date unknown", result.DateAsString);
            Assert.Equal("Met", result.Source);
        }
    }
}
