namespace Cinema.DTOs
{
    public class SalesStatisticsDTO
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public int TicketsSold { get; set; }
        public double Revenue { get; set; }
    }
}
