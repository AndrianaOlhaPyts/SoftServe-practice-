using Cinema.Models.DataBaseModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Repository.Interface;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SalesStatisticsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalesStatisticsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 📌 Відображення завершених сеансів
        public async Task<IActionResult> CompletedSessions()
        {
            var completedSessions = (await _unitOfWork.Sessions.GetAllSessionsAsync())
                .Where(s => s.EndTime <= DateTime.Now); // Отримуємо всі завершені сеанси
            return View(completedSessions);
        }

        // 📌 Перегляд статистики для конкретного сеансу
        public async Task<IActionResult> SessionStatistics(Guid sessionId)
        {
            var existingStats = await _unitOfWork.SalesStatistics
                .GetFirstOrDefaultAsync(s => s.SessionId == sessionId);

            if (existingStats == null)
            {
                var tickets = await _unitOfWork.Tickets
                    .GetAllAsync(t => t.SessionId == sessionId && t.Status == "sold");

                var newStats = new SalesStatistics
                {
                    Id = Guid.NewGuid(),
                    SessionId = sessionId,
                    TicketsSold = tickets.Count(),
                    Revenue = tickets.Sum(t => t.Price)
                };

                await _unitOfWork.SalesStatistics.AddAsync(newStats);
                await _unitOfWork.SaveAsync();

                existingStats = newStats;
            }

            return View(existingStats);
        }
    }
}
