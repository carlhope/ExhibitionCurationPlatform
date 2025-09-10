using ExhibitionCurationPlatform.Context;
using ExhibitionCurationPlatform.Models;
using ExhibitionCurationPlatform.Models.Enums;
using ExhibitionCurationPlatform.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExhibitionCurationPlatform.Repository
{
    public class ExhibitionRepository : IExhibitionRepository
    {
        private readonly ApplicationDbContext _db;

        public ExhibitionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Exhibition> CreateExhibitionAsync(string title, string curator, string description, ExhibitionTheme theme, ExhibitionLayout layout)
        {
            var exhibition = new Exhibition
            {
                Title = title,
                Curator = curator,
                Description = description,
                Theme = theme,
                Layout = layout,
                CreatedAt = DateTime.UtcNow
            };

            _db.Exhibitions.Add(exhibition);
            await _db.SaveChangesAsync();
            return exhibition;
        }

        public async Task<bool> AddArtworkAsync(Guid exhibitionId, Artwork artwork)
        {
            var exhibition = await _db.Exhibitions
                .Include(e => e.Artworks)
                .FirstOrDefaultAsync(e => e.Id == exhibitionId);

            if (exhibition == null)
                return false;

            if (exhibition.Artworks.Any(a => a.Id == artwork.Id))
                return false;

            exhibition.Artworks.Add(artwork);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveArtworkAsync(Guid exhibitionId, string artworkId)
        {
            var exhibition = await _db.Exhibitions
                .Include(e => e.Artworks)
                .FirstOrDefaultAsync(e => e.Id == exhibitionId);

            if (exhibition == null)
                return false;

            var artwork = exhibition.Artworks.FirstOrDefault(a => a.Id == artworkId);
            if (artwork == null)
                return false;

            exhibition.Artworks.Remove(artwork);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Exhibition?> GetExhibitionByIdAsync(Guid exhibitionId)
        {
            return await _db.Exhibitions
                .Include(e => e.Artworks)
                .FirstOrDefaultAsync(e => e.Id == exhibitionId);
        }

        public async Task<List<Exhibition>> GetExhibitionsByCuratorAsync(string curator)
        {
            return await _db.Exhibitions
                .Where(e => e.Curator == curator)
                .Include(e => e.Artworks)
                .ToListAsync();
        }
        public async Task<bool> UpdateAsync(Exhibition exhibition)
        {
            _db.Exhibitions.Update(exhibition);
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }
    }
}
