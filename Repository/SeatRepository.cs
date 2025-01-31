using Cinema.Data;
using Cinema.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Repositories
{
    public class SeatRepository : GenericRepository<Seat>, ISeatRepository
    {
        public SeatRepository(CinemaContext context) : base(context) { }

        public async Task<List<Seat>> GetSeatsByHallIdAsync(Guid hallId)
        {
            return await _context.Seats
                .Include(s => s.Row)
                .Where(s => s.Row.HallId == hallId)
                .ToListAsync();
        }
    }
}
