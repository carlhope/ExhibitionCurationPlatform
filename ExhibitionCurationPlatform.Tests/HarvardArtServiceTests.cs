using ExhibitionCurationPlatform.Config;
using ExhibitionCurationPlatform.Mappers;
using ExhibitionCurationPlatform.Services;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ExhibitionCurationPlatform.Tests
{
    public class HarvardArtServiceTests
    {
        [Fact]
        public async Task SearchAsync_ReturnsMappedArtworks()
        {
            // Arrange
            var json = @"{
            ""records"": [
                {
                    ""title"": ""Test Art"",
                    ""primaryimageurl"": ""http://example.com/image.jpg"",
                    ""people"": [{ ""name"": ""Test Artist"" }],
                    ""id"": ""123"",
                    ""dated"": ""2020""
                }
            ]
        }";

            var handler = new Mock<HttpMessageHandler>();
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(handler.Object);
            var options = Options.Create(new HarvardArtOptions
            {
                ApiKey = "fake-api-key"
            });

            var service = new HarvardArtService(httpClient, options);

            // Act
            var result = await service.SearchAsync("test");

            // Assert
            Assert.Single(result);
            var artwork = result[0];
            Assert.Equal("Test Art", artwork.Title);
            Assert.Equal("Test Artist", artwork.Artist);
            Assert.Equal("http://example.com/image.jpg", artwork.ImageUrl);
            Assert.Equal("Harvard", artwork.Source);
            Assert.Equal("123", artwork.Id);
            Assert.Equal("2020", artwork.Date);
        }
    }
}
