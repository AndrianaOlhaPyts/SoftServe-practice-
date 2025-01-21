namespace Cinema.Models.DataBaseModels
{
    public class Seat
    {
        public Guid Id { get; set; }
        public Guid RowId { get; set; }
        public int SeatNumber { get; set; }
        public string SeatType { get; set; }

        // Навігаційні властивості
        public Row Row { get; set; } = null!;
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
