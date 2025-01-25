namespace Cinema.Models.DataBaseModels
{
    public class Ticket
    {
        public Guid Id { get; set; } // Унікальний ідентифікатор
        public Guid SessionId { get; set; } // Ідентифікатор сеансу
        public Guid SeatId { get; set; } // Ідентифікатор місця
        public string? UserId  { get; set; } // Ідентифікатор користувача
        public int SeatNumber { get; set; } // Номер місця
        public string Status { get; set; } = "booked"; // Статус квитка (наприклад, "booked", "paid", "canceled")
        public double Price { get; set; } // Ціна квитка

        // Навігаційні властивості
        public Session Session { get; set; } = null!; // Зв'язок із сеансом
        public Seat Seat { get; set; } = null!; // Зв'язок із місцем
        public User User { get; set; } = null!; // Зв'язок із користувачем
    }
}
