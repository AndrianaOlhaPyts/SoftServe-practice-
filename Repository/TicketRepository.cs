using Cinema.Data;
using Cinema.Models.DataBaseModels;
using Cinema.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Repositories
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(CinemaContext context) : base(context) { }

        public async Task<List<Ticket>> GetTicketsBySessionIdAsync(Guid sessionId)
        {
            return await _context.Tickets
                .Include(t => t.Seat)
                .Where(t => t.SessionId == sessionId)
                .ToListAsync();
        }
    }
}
