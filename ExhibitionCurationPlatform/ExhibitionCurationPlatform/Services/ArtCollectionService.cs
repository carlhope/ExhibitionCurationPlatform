using ExhibitionCurationPlatform.Models;
using ExhibitionCurationPlatform.Services.Interfaces;

namespace ExhibitionCurationPlatform.Services
{
    public class ArtCollectionService : IArtCollectionService
    {
        private readonly IHarvardArtService _harvard;
        private readonly IMetMuseumService _met;

        public ArtCollectionService(IHarvardArtService harvard, IMetMuseumService met)
        {
            _harvard = harvard;
            _met = met;
        }

        public async Task<PaginatedResult<Artwork>> SearchAsync(
            string query,
            int pageNumber,
            int pageSize,
            string? sortBy = null,
            string? filterBy = null
            )
        {
            var metResults = await _met.SearchAsync(query, filterBy);
            foreach (var artwork in metResults)
                artwork.Source = "MetMuseum";

            var harvardResults = await _harvard.SearchAsync(query, filterBy);
            foreach (var artwork in harvardResults)
                artwork.Source = "HarvardArtMuseums";

            var combined = harvardResults.Concat(metResults).ToList();
            if (!string.IsNullOrEmpty(sortBy))
            {
                combined = sortBy switch
                {
                    "title" => combined.OrderBy(a => a.Title).ToList(),
                    "date" => combined.OrderBy(a => a.Date).ToList(),
                    "artist" => combined.OrderBy(a => a.Artist).ToList(),
                    _ => combined
                };
            }

            var totalCount = combined.Count;

            var pagedItems = combined
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<Artwork>
            {
                Items = pagedItems,
                TotalCount = totalCount
            };
        }
    }
}
