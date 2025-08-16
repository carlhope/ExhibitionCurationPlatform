using ExhibitionCurationPlatform.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExhibitionCurationPlatform.Tests
{
    public class MetMuseumServiceTests
    {
        [Fact]
        public async Task SearchAsync_ReturnsMappedArtworks_WhenApiReturnsValidData()
        {
            // Arrange
            var query = "sunflowers";
            var searchJson = """
        {
            "total": 2,
            "objectIDs": [123, 456]
        }
        """;

            var objectJson = """
        {
            "objectID": 123,
            "title": "Sunflowers",
            "artistDisplayName": "Vincent van Gogh",
            "primaryImageSmall": "https://example.com/sunflowers.jpg"
        }
        """;

            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
             .SetupRequest(HttpMethod.Get, "https://collectionapi.metmuseum.org/public/collection/v1/search?q=sunflowers")
             .ReturnsJson(searchJson);

            handlerMock
                .SetupRequest(HttpMethod.Get, "https://collectionapi.metmuseum.org/public/collection/v1/objects/123")
                .ReturnsJson(objectJson);

            handlerMock
                .SetupRequest(HttpMethod.Get, "https://collectionapi.metmuseum.org/public/collection/v1/objects/456")
                .ReturnsJson(objectJson); // Reuse same mock for simplicity


            var httpClient = new HttpClient(handlerMock.Object);
            var service = new MetMuseumService(httpClient);

            // Act
            var results = await service.SearchAsync(query);

            // Assert
            Assert.Equal(2, results.Count);
            Assert.All(results, a => Assert.Equal("Sunflowers", a.Title));
        }
    }
}
