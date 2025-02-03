namespace Cinema.DTOs
{
    public class RowDTO
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public int Number { get; set; }
        public int SeatsCount { get; set; }
        public string LayoutType { get; set; } = string.Empty;
    }
}
