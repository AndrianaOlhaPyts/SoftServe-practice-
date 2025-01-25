using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Cinema.Data;
using Cinema.Models.DataBaseModels;

namespace Cinema.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        private readonly CinemaContext _context;

        public MovieRepository(CinemaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Movie> GetMovieWithSessionsAsync(Guid movieId)
        {
            return await _context.Movies
                .Include(m => m.Sessions)
                .FirstOrDefaultAsync(m => m.Id == movieId);
        }
    }
}
