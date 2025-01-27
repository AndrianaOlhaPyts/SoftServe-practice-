using Cinema.Models.DataBaseModels;

namespace Cinema.Repositories
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        Task<Movie?> GetMovieWithSessionsAsync(Guid id);
    }
}
