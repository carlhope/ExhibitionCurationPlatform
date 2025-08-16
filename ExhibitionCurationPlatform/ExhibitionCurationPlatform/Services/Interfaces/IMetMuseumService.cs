using ExhibitionCurationPlatform.Models;

namespace ExhibitionCurationPlatform.Services.Interfaces
{
    public interface IMetMuseumService
    {
        Task<List<Artwork>> SearchAsync(string query);

    }
}
