using System.ComponentModel.DataAnnotations;

namespace ExhibitionCurationPlatform.Models
{
    public class Artwork
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Title { get; set; }
        [Required]
        public string Artist { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Date { get; set; }
        public string Source { get; set; } // "Met" or "Harvard"
        public Guid? ExhibitionId { get; set; }//nullable to allow artworks not yet assigned to an exhibition
        public string? CreatedBy { get; set; } // User who added the artwork
        public Exhibition? Exhibition { get; set; }


    }
}
