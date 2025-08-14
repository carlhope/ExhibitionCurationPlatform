using ExhibitionCurationPlatform.Models.Enums;

namespace ExhibitionCurationPlatform.Models
{
    public class Exhibition
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Curator { get; set; } // Optional: could be user name or ID
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ScheduledDate { get; set; } // Optional: for future planning

        public List<Artwork> Artworks { get; set; } = new();
        public ExhibitionLayout Layout { get; set; } = ExhibitionLayout.Grid;
        public ExhibitionTheme Theme { get; set; } = ExhibitionTheme.Modern;
    }
}
