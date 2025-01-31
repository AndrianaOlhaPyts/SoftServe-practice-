using Cinema.Models.DataBaseModels;

namespace Cinema.Repositories
{
    public interface ISeatRepository : IGenericRepository<Seat>
    {
        Task<List<Seat>> GetSeatsByHallIdAsync(Guid hallId);
    }
}