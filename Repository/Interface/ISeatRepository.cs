using Cinema.Models.DataBaseModels;

namespace Cinema.Repository.Interface
{
    public interface ISeatRepository : IGenericRepository<Seat>
    {
        Task<List<Seat>> GetSeatsByHallIdAsync(Guid hallId);
    }
}