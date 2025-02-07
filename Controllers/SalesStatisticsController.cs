using Cinema.Models.DataBaseModels;
using Cinema.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    [Authorize(Roles = "Admin")] // Доступ тільки для адміністратора
    public class SalesStatisticsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalesStatisticsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Statistic()
        {
            var salesStatistics = await _unitOfWork.SalesStatistics.GetAllAsync();

            // Отримуємо всі сеанси та статистику для кожного сеансу
            var sessionStatistics = salesStatistics.Select(stat => new
            {
                stat.Session.Movie.Title,
                stat.Session.StartTime,
                stat.TicketsSold,
                stat.Revenue
            }).ToList();

            // Передаємо дані в представлення
            return View(sessionStatistics);
        }
    }
}
