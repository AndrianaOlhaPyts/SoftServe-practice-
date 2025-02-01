using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Cinema.Repositories;
using Cinema.Data;
using Cinema.Models.DataBaseModels;

namespace Cinema.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.UtcNow;
            var sessions = await _unitOfWork.Sessions.GetAllSessionsAsync();

            var upcomingSessions = sessions
                .Where(s => s.StartTime > today)
                .OrderBy(s => s.StartTime)
                .GroupBy(s => s.Movie)
                .ToList();

            return View(upcomingSessions);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Movies()
        {
            var movies = await _unitOfWork.Movies.GetAllAsync(); // Очікуємо результат
            return View(movies); // Передаємо результат у вигляд
        }
        public async Task<IActionResult> Sessions()
        {
            var sessions = await _unitOfWork.Sessions.GetAllSessionsAsync(); // Очікуємо результат
            return View(sessions); // Передаємо результат у вигляд
        }
        public async Task<IActionResult> ManageTickets(Guid sessionId)
        {
            var tickets = await _unitOfWork.Tickets.GetTicketsBySessionIdAsync(sessionId);
            if (tickets == null || !tickets.Any())
            {
                return NotFound("Квитки не знайдено.");
            }

            return View(tickets);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}