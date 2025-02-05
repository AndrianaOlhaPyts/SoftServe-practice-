using Cinema.Models.DataBaseModels;

namespace Cinema.Repository.Interface
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        Task<Movie?> GetMovieWithSessionsAsync(Guid id);
    }
}
