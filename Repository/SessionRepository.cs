using Cinema.Data;
using Cinema.Models.DataBaseModels;
using Cinema.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Repositories
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(CinemaContext context) : base(context) { }

        public async Task<IEnumerable<Session>> GetSessionsByMovieIdAsync(Guid movieId)
        {
            return await _context.Sessions
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .Where(s => s.MovieId == movieId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Session>> GetAllSessionsAsync()
        {
            return await _context.Sessions
                .Include(s => s.Movie) // Завантажуємо зв’язану таблицю Movie
                .Include(s => s.Hall)  // Завантажуємо зв’язану таблицю Hall
                .ToListAsync();
        }
        public async Task<Session> GetByIdSessionAsync(Guid id)
        {
            var session = await _context.Sessions
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .FirstOrDefaultAsync(s => s.Id == id);
            return session;
        }
    }
}
