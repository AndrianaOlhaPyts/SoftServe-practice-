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
        public async Task<IActionResult> UpdatePrices([FromBody] List<TicketPriceUpdateModel> updates)
        {
            if (updates == null || !updates.Any())
            {
                return BadRequest("No ticket updates provided.");
            }

            foreach (var update in updates)
            {
                var ticket = await _unitOfWork.Tickets.GetByIdAsync(update.TicketId);
                if (ticket == null)
                {
                    return NotFound($"Ticket with ID {update.TicketId} not found.");
                }

                ticket.Price = update.Price;
                await _unitOfWork.Tickets.UpdateAsync(ticket);
            }

            await _unitOfWork.SaveAsync();

            return Ok(new { success = true });
        }


        // Модель для прийому JSON-запиту
        public class TicketPriceUpdateModel
        {
            public Guid TicketId { get; set; }
            public double Price { get; set; }
        }

    }
}
