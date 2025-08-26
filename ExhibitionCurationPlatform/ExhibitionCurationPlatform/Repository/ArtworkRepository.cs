using ExhibitionCurationPlatform.Context;
using ExhibitionCurationPlatform.Models;
using ExhibitionCurationPlatform.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExhibitionCurationPlatform.Repository
{
    public class ArtworkRepository : IArtworkRepository
    {
        private readonly ApplicationDbContext _context;

        public ArtworkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Artwork> CreateAsync(Artwork artwork)
        {
            _context.Artworks.Add(artwork);
            await _context.SaveChangesAsync();
            return artwork;
        }

        public async Task<Artwork?> GetByIdAsync(string id)
        {
            return await _context.Artworks.FindAsync(id);
        }

        public async Task<List<Artwork>> GetAllByUserIdAsync(string userId)
        {
            return await _context.Artworks
                .Where(a => a.Source == "User" && a.CreatedBy == userId)
                .ToListAsync();
        }
    }
}
