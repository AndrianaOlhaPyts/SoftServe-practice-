namespace Cinema.Models.DataBaseModels
{
    public class Row
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public int SeatsCount { get; set; }
        public string LayoutType { get; set; }

        // Навігаційні властивості
        public Hall Hall { get; set; } = null!;
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
