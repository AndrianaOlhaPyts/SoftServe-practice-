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
            Halls = new GenericRepository<Hall>(_context);
            Rows = new GenericRepository<Row>(_context);
            Seats = new GenericRepository<Seat>(_context);
            Sessions = new GenericRepository<Session>(_context);
            Tickets = new GenericRepository<Ticket>(_context);
            SalesStatistics = new GenericRepository<SalesStatistics>(_context);
        }

        public IMovieRepository Movies { get; private set; }
        public IGenericRepository<Hall> Halls { get; private set; }
        public IGenericRepository<Row> Rows { get; private set; }
        public IGenericRepository<Seat> Seats { get; private set; }
        public IGenericRepository<Session> Sessions { get; private set; }
        public IGenericRepository<Ticket> Tickets { get; private set; }
        public IGenericRepository<SalesStatistics> SalesStatistics { get; private set; }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
