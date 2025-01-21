namespace Cinema.Models.DataBaseModels
{
    public class Hall
    {
        public Guid Id { get; set; } // Унікальний ідентифікатор
        public string Name { get; set; } // Назва залу
        public string Description { get; set; } //Опис

        // Навігаційні властивості
        public ICollection<Row> Rows { get; set; } = new List<Row>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
