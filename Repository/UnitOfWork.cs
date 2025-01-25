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
            Halls = new Repository<Hall>(_context);
            Rows = new Repository<Row>(_context);
            Seats = new Repository<Seat>(_context);
            Sessions = new Repository<Session>(_context);
            Tickets = new Repository<Ticket>(_context);
            SalesStatistics = new Repository<SalesStatistics>(_context);
        }

        public IMovieRepository Movies { get; private set; }
        public IRepository<Hall> Halls { get; private set; }
        public IRepository<Row> Rows { get; private set; }
        public IRepository<Seat> Seats { get; private set; }
        public IRepository<Session> Sessions { get; private set; }
        public IRepository<Ticket> Tickets { get; private set; }
        public IRepository<SalesStatistics> SalesStatistics { get; private set; }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
