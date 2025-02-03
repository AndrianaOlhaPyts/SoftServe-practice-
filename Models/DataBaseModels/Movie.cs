namespace Cinema.Models.DataBaseModels
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string Genre { get; set; } = string.Empty;
        public string TrailerUrl { get; set; } = string.Empty;
        public string PosterUrl { get; set; } = string.Empty;
        public decimal Rating { get; set; }

        // Навігаційні властивостіm
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
