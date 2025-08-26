using ExhibitionCurationPlatform.Models;

namespace ExhibitionCurationPlatform.Repository.Interfaces
{
    public interface IArtworkRepository
    {
        Task<Artwork> CreateAsync(Artwork artwork);
        Task<Artwork?> GetByIdAsync(string id);
        Task<List<Artwork>> GetAllByUserIdAsync(string userId);
    }
}
