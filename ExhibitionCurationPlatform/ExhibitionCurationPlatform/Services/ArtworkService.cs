using ExhibitionCurationPlatform.Context;
using ExhibitionCurationPlatform.Models;
using ExhibitionCurationPlatform.Repository.Interfaces;
using ExhibitionCurationPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ExhibitionCurationPlatform.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly IArtworkRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ArtworkService(IArtworkRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<Artwork> CreateAsync(Artwork artwork)
        {
            artwork.Source = "User";
            return await _repository.CreateAsync(artwork);
        }

        public async Task<Artwork?> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Artwork>> GetUserArtworksAsync(string userId)
        {
            return await _repository.GetAllByUserIdAsync(userId);
        }
    }

}
