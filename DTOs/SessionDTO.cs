namespace Cinema.DTOs
{
    public class SessionDTO
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid HallId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
    }
}
