using Cinema.Models.DataBaseModels;
using Cinema.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
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


        // 📌 Відображення доступних місць для сеансу
        [Authorize]
        public async Task<IActionResult> ClientManageTickets(Guid sessionId)
        {
            var tickets = await _unitOfWork.Tickets.GetTicketsBySessionIdAsync(sessionId);

            if (tickets == null || !tickets.Any())
            {
                return NotFound("Квитки не знайдено.");
            }

            return View(tickets);
        }

        // 📌 Підтвердження вибору місць
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ConfirmSelection([FromBody] List<TicketSelectionModel> selectedSeats)
        {
            if (selectedSeats == null || !selectedSeats.Any())
            {
                return BadRequest("No seats selected.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID
            if (userId == null)
            {
                return Unauthorized("Користувач не авторизований.");
            }

            foreach (var selection in selectedSeats)
            {
                var ticket = await _unitOfWork.Tickets.GetByIdAsync(selection.TicketId);
                if (ticket == null)
                {
                    return NotFound($"Ticket with ID {selection.TicketId} not found.");
                }

                // Check if the ticket is already booked
                if (ticket.IsBooked)
                {
                    return Conflict($"Місце {ticket.Seat.SeatNumber} вже заброньоване.");
                }

                // Update the booking status
                ticket.IsBooked = true;
                ticket.UserId = userId; // Bind the booking to the user
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


        public async Task<IActionResult> Payment(double totalPrice, string ticketIds)
        {
            ViewData["TotalPrice"] = totalPrice;
            ViewData["TicketIds"] = ticketIds;
            return View();
        }


        // 📌 Обробка платежу
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentModel payment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("Користувач не авторизований.");
            }

            if (payment == null || payment.TicketIds == null || !payment.TicketIds.Any() || payment.Amount <= 0)
            {
                return BadRequest("Invalid payment details.");
            }

            foreach (var ticketId in payment.TicketIds)
            {
                var ticket = await _unitOfWork.Tickets.GetByIdAsync(ticketId);
                if (ticket != null && ticket.UserId == userId && !ticket.IsPaid)
                {
                    ticket.IsPaid = true;  // Помічаємо квиток як оплачений
                    ticket.Status = "sold"; // Оновлюємо статус
                    await _unitOfWork.Tickets.UpdateAsync(ticket);
                }
            }

            await _unitOfWork.SaveAsync();
            return Ok(new { success = true });
        }

        // Модель для обробки платежу
        public class PaymentModel
        {
            public List<Guid> TicketIds { get; set; }
            public double Amount { get; set; }
        }

    }
}
