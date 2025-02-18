namespace Cinema.DTOs
{
    public class SeatDTO
    {
        public Guid Id { get; set; }
        public Guid RowId { get; set; }
        public int SeatNumber { get; set; }
        public string SeatType { get; set; } = "standard";

        public RowDTO Row { get; set; }
    }
}
