using ExhibitionCurationPlatform.Models;
using ExhibitionCurationPlatform.Services.Interfaces;
using ExhibitionCurationPlatform.Static_Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var metTask = _met.SearchAsync(query, filterBy);
            var harvardTask = _harvard.SearchAsync(query, filterBy);

            await Task.WhenAll(metTask, harvardTask);

            var metResults = metTask.Result;
            foreach (var artwork in metResults)
                artwork.Source = "MetMuseum";

            var harvardResults = harvardTask.Result;
            foreach (var artwork in harvardResults)
                artwork.Source = "HarvardArtMuseums";

            var combined = harvardResults.Concat(metResults).ToList();
            if (!string.IsNullOrEmpty(sortBy))
            {
                combined = sortBy switch
                {
                    SortOptions.Title => combined.OrderBy(a => a.Title).ToList(),
                    SortOptions.Date => combined.OrderByDescending(a => a.Date.Year >0002?a.Date:DateOnly.MinValue).ToList(),
                    SortOptions.Artist => combined.OrderBy(a => a.Artist).ToList(),
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