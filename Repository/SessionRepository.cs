using Cinema.Data;
using Cinema.Models.DataBaseModels;
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
    }
}
