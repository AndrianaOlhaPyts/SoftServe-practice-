namespace Cinema.DTOs
{
    public class TicketDTO
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public Guid SeatId { get; set; }
        public string? UserId { get; set; }
        public int SeatNumber { get; set; }
        public string Status { get; set; } = "booked";
        public double Price { get; set; }
    }
}
