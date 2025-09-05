using ExhibitionCurationPlatform.Models;

namespace ExhibitionCurationPlatform.Services.Interfaces
{
    public interface IMetMuseumService
    {
        Task<List<Artwork>> SearchAsync(string query, string? filterBy);
        Task<Artwork?> GetByIdAsync(string id);


    }
}
