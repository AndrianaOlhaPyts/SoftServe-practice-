namespace Cinema.Models.DataBaseModels
{
    public class Session
    {
        public Guid Id { get; set; } // Унікальний ідентифікатор
        public Guid MovieId { get; set; } // Ідентифікатор фільму
        public Guid HallId { get; set; } // Ідентифікатор залу
        public DateTimeOffset StartTime { get; set; } // Час початку сеансу
        public DateTimeOffset EndTime { get; set; } // Час завершення сеансу

        // Навігаційні властивості
        public Movie Movie { get; set; } = null!;
        public Hall Hall { get; set; } = null!;
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public SalesStatistics ? SalesStatistics { get; set; } 
    }
}
