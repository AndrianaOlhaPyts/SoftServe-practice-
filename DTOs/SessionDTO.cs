namespace Cinema.DTOs
{
    public class SessionDTO
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public string MovieTitle { get; set; }  // Додана властивість для назви фільму
        public Guid HallId { get; set; }
        public string HallName { get; set; }  // Додана властивість для назви залу
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
    }
}
