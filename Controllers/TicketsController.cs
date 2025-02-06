using Cinema.Models.DataBaseModels;
using Cinema.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
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



        public async Task<IActionResult> ClientManageTickets(Guid sessionId)
        {
            var tickets = await _unitOfWork.Tickets.GetTicketsBySessionIdAsync(sessionId);
            if (tickets == null || !tickets.Any())
            {
                return NotFound("Квитки не знайдено.");
            }

            return View(tickets);  // Повертаємо view для клієнта
        }

        // 📌 Оновлення вибору місць
        [HttpPost]
        public async Task<IActionResult> ConfirmSelection([FromBody] List<TicketSelectionModel> selectedSeats)
        {
            if (selectedSeats == null || !selectedSeats.Any())
            {
                return BadRequest("No seats selected.");
            }

            foreach (var selection in selectedSeats)
            {
                var ticket = await _unitOfWork.Tickets.GetByIdAsync(selection.TicketId);
                if (ticket == null)
                {
                    return NotFound($"Ticket with ID {selection.TicketId} not found.");
                }

                // Оновлюємо статус місця, наприклад, позначаємо як вибране
                ticket.IsSelected = true; // Це поле потрібно додати в модель Ticket
                await _unitOfWork.Tickets.UpdateAsync(ticket);
            }

            await _unitOfWork.SaveAsync();
            return Ok(new { success = true });
        }

        // Модель для прийому даних вибору місць
        public class TicketSelectionModel
        {
            public Guid TicketId { get; set; }
            public string SeatNumber { get; set; }
            public string SeatType { get; set; }
        }
    }
}
