using Cinema.Models.DataBaseModels;
using System.Threading.Tasks;

namespace Cinema.Repositories
{
    public interface IUnitOfWork
    {
        IMovieRepository Movies { get; }
        IGenericRepository<Hall> Halls { get; }
        IGenericRepository<Row> Rows { get; }
        IGenericRepository<Seat> Seats { get; }
        ISessionRepository Sessions { get; }
        IGenericRepository<Ticket> Tickets { get; }
        IGenericRepository<SalesStatistics> SalesStatistics { get; }
        Task SaveAsync();
    }
}
