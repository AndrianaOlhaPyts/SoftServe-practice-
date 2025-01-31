using Cinema.Models.DataBaseModels;
using System.Threading.Tasks;

namespace Cinema.Repositories
{
    public interface IUnitOfWork
    {
        IMovieRepository Movies { get; }
        IHallRepository Halls { get; }
        IGenericRepository<Row> Rows { get; }
        ISeatRepository Seats { get; }
        ISessionRepository Sessions { get; }
        ITicketRepository Tickets { get; }
        IGenericRepository<SalesStatistics> SalesStatistics { get; }
        Task SaveAsync();
    }
}
