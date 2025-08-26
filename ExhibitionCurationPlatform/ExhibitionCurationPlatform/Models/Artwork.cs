namespace ExhibitionCurationPlatform.Models
{
    public class Artwork
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; }
        public string Artist { get; set; }
        public string ImageUrl { get; set; }
        public string Date { get; set; }
        public string Source { get; set; } // "Met" or "Harvard"
        public Guid? ExhibitionId { get; set; }//nullable to allow artworks not yet assigned to an exhibition
        public string? CreatedBy { get; set; } // User who added the artwork
        public Exhibition? Exhibition { get; set; }


    }
}
