using Cinema.Models.DataBaseModels;
using Cinema.Repositories;

namespace Cinema.Services
{
    public class AdminService : IAdminService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ISessionRepository _sessionRepository;

        public AdminService(IMovieRepository movieRepository, ISessionRepository sessionRepository)
        {
            _movieRepository = movieRepository;
            _sessionRepository = sessionRepository;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync() => await _movieRepository.GetAllAsync();
        public async Task<Movie?> GetMovieByIdAsync(Guid id) => await _movieRepository.GetMovieWithSessionsAsync(id);
        public async Task CreateMovieAsync(Movie movie) => await _movieRepository.AddAsync(movie);
        public async Task UpdateMovieAsync(Movie movie) => await _movieRepository.UpdateAsync(movie);
        public async Task DeleteMovieAsync(Guid id) => await _movieRepository.DeleteAsync(id);

        public async Task<IEnumerable<Session>> GetAllSessionsAsync() => await _sessionRepository.GetAllAsync();
        public async Task<Session?> GetSessionByIdAsync(Guid id) => await _sessionRepository.GetByIdAsync(id);
        public async Task CreateSessionAsync(Session session) => await _sessionRepository.AddAsync(session);
        public async Task UpdateSessionAsync(Session session) => await _sessionRepository.UpdateAsync(session);
        public async Task DeleteSessionAsync(Guid id) => await _sessionRepository.DeleteAsync(id);
    }
}
