using ExhibitionCurationPlatform.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExhibitionCurationPlatform.Tests
{
    public class HarvardMapperTests
    {
        [Fact]
        public void FromHarvardJson_MapsAllFieldsCorrectly()
        {
            var json = JsonDocument.Parse("""
    {
        "id": "abc123",
        "title": "Sunflowers",
        "people": [{ "name": "Van Gogh" }],
        "primaryimageurl": "http://example.com/image.jpg",
        "dated": "1888"
    }
    """).RootElement;

            var result = ArtworkMapper.FromHarvardJson(json);

            Assert.Equal("abc123", result.Id);
            Assert.Equal("Sunflowers", result.Title);
            Assert.Equal("Van Gogh", result.Artist);
            Assert.Equal("http://example.com/image.jpg", result.ImageUrl);
            Assert.Equal("1888", result.Date);
            Assert.Equal("Harvard", result.Source);
        }

        [Fact]
        public void FromHarvardJson_HandlesEmptyPeopleArray()
        {
            var json = JsonDocument.Parse("""
    {
        "id": "abc123",
        "title": "Sunflowers",
        "people": [],
        "primaryimageurl": "http://example.com/image.jpg",
        "dated": "1888"
    }
    """).RootElement;

            var result = ArtworkMapper.FromHarvardJson(json);

            Assert.Equal("Unknown", result.Artist);
        }
    }
}
