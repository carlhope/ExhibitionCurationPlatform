using ExhibitionCurationPlatform.Models;

namespace ExhibitionCurationPlatform.Services.Interfaces
{
    public interface IExhibitionService
    {
        Task<Exhibition> CreateExhibitionAsync(string title, string curator, string description);
        Task<bool> AddArtworkAsync(Guid exhibitionId, Artwork artwork);
        Task<bool> RemoveArtworkAsync(Guid exhibitionId, string artworkId);
        Task<List<Exhibition>> GetExhibitionsByCuratorAsync(string curator);
        Task<Exhibition?> GetExhibitionByIdAsync(Guid exhibitionId);
    }
}
