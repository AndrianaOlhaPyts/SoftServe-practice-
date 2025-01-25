using System.Threading.Tasks;
using Cinema.Models.DataBaseModels;

namespace Cinema.Repositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<Movie> GetMovieWithSessionsAsync(Guid movieId);
    }
}
