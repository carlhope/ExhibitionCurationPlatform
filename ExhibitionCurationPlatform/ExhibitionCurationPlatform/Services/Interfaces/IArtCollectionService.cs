using ExhibitionCurationPlatform.Models;

namespace ExhibitionCurationPlatform.Services.Interfaces
{
    public interface IArtCollectionService
    {
        Task<PaginatedResult<Artwork>> SearchAsync(string query, int pageNumber, int pageSize, string? sortBy, string? filterBy);

    }
}
