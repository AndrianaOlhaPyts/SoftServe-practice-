using Cinema.Data;
using Cinema.Models.DataBaseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinema.Repositories
{
    public class HallRepository : GenericRepository<Hall>, IHallRepository
    {
        public HallRepository(CinemaContext context) : base(context) { }

        public async Task<Hall> GetHallByIdAsync(Guid hallId)
        {
            return await _context.Halls
                .Include(h => h.Sessions)
                .FirstOrDefaultAsync(h => h.Id == hallId);
        }
    }
}
