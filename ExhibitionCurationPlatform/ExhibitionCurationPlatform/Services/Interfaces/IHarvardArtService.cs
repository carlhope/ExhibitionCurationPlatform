using ExhibitionCurationPlatform.Models;

namespace ExhibitionCurationPlatform.Services.Interfaces
{
    public interface IHarvardArtService
    {
        Task<List<Artwork>> SearchAsync(string query, string? filterBy);

    }
}
