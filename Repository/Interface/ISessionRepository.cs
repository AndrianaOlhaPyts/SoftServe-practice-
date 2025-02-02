using Cinema.Models.DataBaseModels;

namespace Cinema.Repository.Interface
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetSessionsByMovieIdAsync(Guid movieId);
        Task<IEnumerable<Session>> GetAllSessionsAsync();
        Task<Session> GetByIdSessionAsync(Guid id);
    }
}
