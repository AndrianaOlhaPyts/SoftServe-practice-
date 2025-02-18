using Cinema.Data;
using Cinema.Models.DataBaseModels;
using Cinema.Repository.Interface;
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
                .ThenInclude(s => s.Row)
                .Where(t => t.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<List<Ticket>> GetUserActiveTicketsAsync(string userId)
        {
            return await _context.Tickets
                .Include(t => t.Session)
                .Include(t => t.Seat)
                    .ThenInclude(s => s.Row)
                .Where(t => t.Status == "paid")
                .Include(t => t.Session.Movie)
                .Include(t => t.Session.Hall)
                .Where(t => t.UserId == userId && t.Session.EndTime > DateTime.Now) // Тільки активні сесії
                .ToListAsync();
        }

    }
}
