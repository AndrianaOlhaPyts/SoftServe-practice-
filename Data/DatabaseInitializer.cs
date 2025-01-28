using Cinema.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Data
{
    public class DatabaseInitializer
    {
        private readonly CinemaContext _context;

        public DatabaseInitializer(CinemaContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            // Перевірка, чи вже існує хол із назвою "Main Hall"
            if (!await _context.Halls.AnyAsync(h => h.Name == "1 Hall"))
            {
                // Створення унікального ідентифікатора для холу
                var hallId = Guid.NewGuid();

                // Створення нового холу
                var hall = new Hall
                {
                    Id = hallId,
                    Name = "1 Hall",
                    Description = "Перший хол з з різною розсадкою.",
                    Rows = new List<Row>
                    {
                        // Перші 4 ряди зі стандартною розсадкою, по 11 місць
                        CreateRow(1, 11, "standard", hallId),
                        CreateRow(2, 11, "standard", hallId),
                        CreateRow(3, 11, "standard", hallId),
                        CreateRow(4, 11, "standard", hallId),

                        // Один ряд із VIP розсадкою, 8 VIP місць
                        CreateRow(5, 8, "vip", hallId),

                        // Один ряд зі змішаною розсадкою: 9 стандартних місць, 2 місця для інвалідів
                        CreateRow(6, 9, "standard", hallId, 2, "accessible")
                    }
                };

                // Додавання холу до бази
                await _context.Halls.AddAsync(hall);
                await _context.SaveChangesAsync();
            }
        }

        private Row CreateRow(int number,int standardSeats,string standardType,Guid hallId,int accessibleSeats = 0,string accessibleType = "")
        {
            var rowId = Guid.NewGuid(); // Унікальний ідентифікатор для ряду
            var seats = new List<Seat>();

            // Додавання стандартних місць
            for (int i = 1; i <= standardSeats; i++)
            {
                seats.Add(new Seat
                {
                    Id = Guid.NewGuid(),
                    RowId = rowId, // Встановлюємо RowId для кожного місця
                    SeatNumber = i,
                    SeatType = standardType
                });
            }

            // Додавання місць для інвалідів
            for (int i = 1; i <= accessibleSeats; i++)
            {
                seats.Add(new Seat
                {
                    Id = Guid.NewGuid(),
                    RowId = rowId, // Встановлюємо RowId для кожного місця
                    SeatNumber = standardSeats + i,
                    SeatType = accessibleType
                });
            }

            // Створення ряду
            return new Row
            {
                Id = rowId,
                HallId = hallId, // Встановлюємо HallId для ряду
                Number = number,
                SeatsCount= standardSeats+ accessibleSeats,
                LayoutType = standardType,
                Seats = seats
            };
        }
    }
}
