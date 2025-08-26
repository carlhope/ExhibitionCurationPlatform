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

        public async Task<List<Artwork>> SearchAsync(string query)
        {
            
            var metResults = await _met.SearchAsync(query);
            foreach (var artwork in metResults)
            {
                artwork.Source = "MetMuseum";
            }
            var harvardResults = await _harvard.SearchAsync(query);
            foreach (var artwork in harvardResults)
            {
                artwork.Source = "HarvardArtMuseums";
            }

            return harvardResults.Concat(metResults).ToList();
        }
    }
}
