using Cinema.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cinema.Repositories
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        Task<List<Ticket>> GetTicketsBySessionIdAsync(Guid sessionId);
    }
}
