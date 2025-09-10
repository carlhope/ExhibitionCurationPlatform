using ExhibitionCurationPlatform.Context;
using ExhibitionCurationPlatform.Models;
using ExhibitionCurationPlatform.Models.Enums;
using ExhibitionCurationPlatform.Repository.Interfaces;
using ExhibitionCurationPlatform.Services.Interfaces;

namespace ExhibitionCurationPlatform.Services
{
    public class ExhibitionService : IExhibitionService
    {
        private readonly IExhibitionRepository _exhibitionRepository;

        public ExhibitionService(IExhibitionRepository exhibitionRepository)
        {
            _exhibitionRepository = exhibitionRepository;
        }

        public async Task<Exhibition> CreateExhibitionAsync(string title, string curator, string description, ExhibitionTheme theme, ExhibitionLayout layout)
        {
            return await _exhibitionRepository.CreateExhibitionAsync(title, curator, description, theme, layout);
        }

        public async Task<Exhibition?> GetExhibitionByIdAsync(Guid exhibitionId)
        {
            return await _exhibitionRepository.GetExhibitionByIdAsync(exhibitionId);
        }

        public async Task<List<Exhibition>> GetExhibitionsByCuratorAsync(string curator)
        {
            return await _exhibitionRepository.GetExhibitionsByCuratorAsync(curator);
        }

        public async Task<bool> AddArtworkAsync(Guid exhibitionId, Artwork artwork)
        {
            var exhibition = await _exhibitionRepository.GetExhibitionByIdAsync(exhibitionId);
            if (exhibition == null) return false;

            if (exhibition.Artworks.Any(a => a.Id == artwork.Id))
                return false; // Avoid duplicates

            exhibition.Artworks.Add(artwork);
            return await _exhibitionRepository.UpdateAsync(exhibition);
        }

        public async Task<bool> RemoveArtworkAsync(Guid exhibitionId, string artworkId)
        {
            var exhibition = await _exhibitionRepository.GetExhibitionByIdAsync(exhibitionId);
            if (exhibition == null) return false;

            var artwork = exhibition.Artworks.FirstOrDefault(a => a.Id == artworkId);
            if (artwork == null) return false;

            exhibition.Artworks.Remove(artwork);
            return await _exhibitionRepository.UpdateAsync(exhibition);
        }
    }
}
