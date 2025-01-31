using System.Threading.Tasks;
using Cinema.Data;
using Cinema.Models.DataBaseModels;

namespace Cinema.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CinemaContext _context;

        public UnitOfWork(CinemaContext context)
        {
            _context = context;
            Movies = new MovieRepository(_context);
            Halls = new HallRepository(_context);
            Rows = new GenericRepository<Row>(_context);
            Seats = new SeatRepository(_context);
            Sessions = new SessionRepository(_context);
            Tickets = new TicketRepository(_context);
            SalesStatistics = new GenericRepository<SalesStatistics>(_context);
        }

        public IMovieRepository Movies { get; private set; }
        public IHallRepository Halls { get; private set; }
        public IGenericRepository<Row> Rows { get; private set; }
        public ISeatRepository Seats { get; private set; }
        public ISessionRepository Sessions { get; private set; }
        public ITicketRepository Tickets { get; private set; }
        public IGenericRepository<SalesStatistics> SalesStatistics { get; private set; }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
