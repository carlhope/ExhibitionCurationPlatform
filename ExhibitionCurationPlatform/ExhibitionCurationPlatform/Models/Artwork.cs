namespace ExhibitionCurationPlatform.Models
{
    public class Artwork
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string ImageUrl { get; set; }
        public string Date { get; set; }
        public string Source { get; set; } // "Met" or "Harvard"
    }
}
