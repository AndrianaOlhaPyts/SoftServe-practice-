using Cinema.Models.DataBaseModels;

namespace Cinema.Repositories
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetSessionsByMovieIdAsync(Guid movieId);
    }
}
