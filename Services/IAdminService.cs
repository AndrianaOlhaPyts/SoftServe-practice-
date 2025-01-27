using Cinema.Models.DataBaseModels;

namespace Cinema.Services
{
    public interface IAdminService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie?> GetMovieByIdAsync(Guid id);
        Task CreateMovieAsync(Movie movie);
        Task UpdateMovieAsync(Movie movie);
        Task DeleteMovieAsync(Guid id);

        Task<IEnumerable<Session>> GetAllSessionsAsync();
        Task<Session?> GetSessionByIdAsync(Guid id);
        Task CreateSessionAsync(Session session);
        Task UpdateSessionAsync(Session session);
        Task DeleteSessionAsync(Guid id);
    }
}
