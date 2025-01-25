using Cinema.Models.DataBaseModels;
using System.Threading.Tasks;

namespace Cinema.Repositories
{
    public interface IUnitOfWork
    {
        IMovieRepository Movies { get; }
        IRepository<Hall> Halls { get; }
        IRepository<Row> Rows { get; }
        IRepository<Seat> Seats { get; }
        IRepository<Session> Sessions { get; }
        IRepository<Ticket> Tickets { get; }
        IRepository<SalesStatistics> SalesStatistics { get; }
        Task SaveAsync();
    }
}
