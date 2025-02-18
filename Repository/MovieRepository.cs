using Cinema.Data;
using Cinema.Models.DataBaseModels;
using Cinema.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Repositories
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(CinemaContext context) : base(context) { }

        public async Task<Movie?> GetMovieWithSessionsAsync(Guid id)
        {
            return await _context.Movies
                .Include(m => m.Sessions)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
