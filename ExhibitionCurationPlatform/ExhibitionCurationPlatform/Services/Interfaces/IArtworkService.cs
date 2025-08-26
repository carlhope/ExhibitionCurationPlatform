using ExhibitionCurationPlatform.Models;

namespace ExhibitionCurationPlatform.Services.Interfaces
{
    public interface IArtworkService
    {
        Task<Artwork> CreateAsync(Artwork artwork);
        Task<Artwork?> GetByIdAsync(string id);
        Task<List<Artwork>> GetUserArtworksAsync(string userId);

    }
}
