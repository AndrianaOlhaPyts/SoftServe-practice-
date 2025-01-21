namespace Cinema.Models.DataBaseModels
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        // Навігаційні властивості
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
