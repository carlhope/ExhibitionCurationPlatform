using Xunit;
using Moq;
using ExhibitionCurationPlatform.Models;
using ExhibitionCurationPlatform.Repository.Interfaces;
using ExhibitionCurationPlatform.Services;


namespace ExhibitionCurationPlatform.Tests
{

    public class ExhibitionServiceTests
    {
        private readonly Mock<IExhibitionInterface> _mockRepo;
        private readonly ExhibitionService _service;

        public ExhibitionServiceTests()
        {
            _mockRepo = new Mock<IExhibitionInterface>();
            _service = new ExhibitionService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateExhibitionAsync_ShouldCallRepositoryAndReturnExhibition()
        {
            // Arrange
            var expected = new Exhibition { Title = "Impressionism", Curator = "Carl" };
            _mockRepo.Setup(r => r.CreateExhibitionAsync("Impressionism", "Carl", "A study in light"))
                     .ReturnsAsync(expected);

            // Act
            var result = await _service.CreateExhibitionAsync("Impressionism", "Carl", "A study in light");

            // Assert
            Assert.Equal(expected, result);
            _mockRepo.Verify(r => r.CreateExhibitionAsync("Impressionism", "Carl", "A study in light"), Times.Once);
        }

        [Fact]
        public async Task GetExhibitionByIdAsync_ShouldReturnExhibition()
        {
            var id = Guid.NewGuid();
            var exhibition = new Exhibition { Id = id };
            _mockRepo.Setup(r => r.GetExhibitionByIdAsync(id)).ReturnsAsync(exhibition);

            var result = await _service.GetExhibitionByIdAsync(id);

            Assert.Equal(exhibition, result);
        }

        [Fact]
        public async Task GetExhibitionsByCuratorAsync_ShouldReturnList()
        {
            var exhibitions = new List<Exhibition> { new Exhibition { Curator = "Carl" } };
            _mockRepo.Setup(r => r.GetExhibitionsByCuratorAsync("Carl")).ReturnsAsync(exhibitions);

            var result = await _service.GetExhibitionsByCuratorAsync("Carl");

            Assert.Single(result);
            Assert.Equal("Carl", result[0].Curator);
        }

        [Fact]
        public async Task AddArtworkAsync_ShouldAddArtwork_WhenNotDuplicate()
        {
            var id = Guid.NewGuid();
            var artwork = new Artwork { Id = "A1" };
            var exhibition = new Exhibition { Id = id, Artworks = new List<Artwork>() };

            _mockRepo.Setup(r => r.GetExhibitionByIdAsync(id)).ReturnsAsync(exhibition);
            _mockRepo.Setup(r => r.UpdateAsync(exhibition)).ReturnsAsync(true);

            var result = await _service.AddArtworkAsync(id, artwork);

            Assert.True(result);
            Assert.Contains(artwork, exhibition.Artworks);
            _mockRepo.Verify(r => r.UpdateAsync(exhibition), Times.Once);
        }

        [Fact]
        public async Task AddArtworkAsync_ShouldNotAdd_WhenDuplicate()
        {
            var id = Guid.NewGuid();
            var artwork = new Artwork { Id = "A1" };
            var exhibition = new Exhibition { Id = id, Artworks = new List<Artwork> { artwork } };

            _mockRepo.Setup(r => r.GetExhibitionByIdAsync(id)).ReturnsAsync(exhibition);

            var result = await _service.AddArtworkAsync(id, artwork);

            Assert.False(result);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Exhibition>()), Times.Never);
        }

        [Fact]
        public async Task RemoveArtworkAsync_ShouldRemove_WhenExists()
        {
            var id = Guid.NewGuid();
            var artwork = new Artwork { Id = "A1" };
            var exhibition = new Exhibition { Id = id, Artworks = new List<Artwork> { artwork } };

            _mockRepo.Setup(r => r.GetExhibitionByIdAsync(id)).ReturnsAsync(exhibition);
            _mockRepo.Setup(r => r.UpdateAsync(exhibition)).ReturnsAsync(true);

            var result = await _service.RemoveArtworkAsync(id, "A1");

            Assert.True(result);
            Assert.DoesNotContain(artwork, exhibition.Artworks);
        }

        [Fact]
        public async Task RemoveArtworkAsync_ShouldReturnFalse_WhenArtworkNotFound()
        {
            var id = Guid.NewGuid();
            var exhibition = new Exhibition { Id = id, Artworks = new List<Artwork>() };

            _mockRepo.Setup(r => r.GetExhibitionByIdAsync(id)).ReturnsAsync(exhibition);

            var result = await _service.RemoveArtworkAsync(id, "A1");

            Assert.False(result);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Exhibition>()), Times.Never);
        }
    }
}
