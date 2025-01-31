using Cinema.Models.DataBaseModels;
using Cinema.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 📌 Відображення сторінки керування квитками
        public async Task<IActionResult> ManageTickets(Guid sessionId)
        {
            var tickets = await _unitOfWork.Tickets.GetTicketsBySessionIdAsync(sessionId);
            if (tickets == null || !tickets.Any())
            {
                return NotFound("Квитки не знайдено.");
            }

            return View(tickets);
        }

        // 📌 Оновлення ціни квитка
        [HttpPost]
        public async Task<IActionResult> UpdatePrice(Guid ticketId, double price)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
            if (ticket == null) return NotFound();

            ticket.Price = price;
            await _unitOfWork.Tickets.UpdateAsync(ticket);
            await _unitOfWork.SaveAsync();

            return Ok(new { success = true });
        }
    }
}
