using AutoMapper;
using Cinema.DTOs;
using Cinema.Models;
using Cinema.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Cinema.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.UtcNow;
            var sessions = await _unitOfWork.Sessions.GetAllSessionsAsync();

            var upcomingSessions = sessions
                .Where(s => s.StartTime > today)
                .OrderBy(s => s.StartTime)
                .GroupBy(s => s.Movie)
                .Select(group => new
                {
                    Movie = _mapper.Map<MovieDTO>(group.Key),
                    Sessions = _mapper.Map<List<SessionDTO>>(group.ToList())
                })
                .ToList();

            return View(upcomingSessions);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Movies()
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var movieDTOs = _mapper.Map<List<MovieDTO>>(movies);
            return View(movieDTOs);
        }

        public async Task<IActionResult> Sessions()
        {
            var sessions = await _unitOfWork.Sessions.GetAllSessionsAsync();
            var sessionDTOs = _mapper.Map<List<SessionDTO>>(sessions);
            return View(sessionDTOs);
        }

        public async Task<IActionResult> ManageTickets(Guid sessionId)
        {
            var tickets = await _unitOfWork.Tickets.GetTicketsBySessionIdAsync(sessionId);
            if (tickets == null || !tickets.Any())
            {
                return NotFound("Квитки не знайдено.");
            }

            var ticketDTOs = _mapper.Map<List<TicketDTO>>(tickets);
            return View(ticketDTOs);
        }

        public async Task<IActionResult> CompletedSessions()
        {
            var completedSessions = (await _unitOfWork.Sessions.GetAllSessionsAsync())
                .Where(s => s.EndTime <= DateTime.Now);

            var completedSessionDTOs = _mapper.Map<List<SessionDTO>>(completedSessions);
            return View(completedSessionDTOs);
        }

        public async Task<IActionResult> Tickets()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Отримуємо ID поточного користувача

            var tickets = await _unitOfWork.Tickets.GetUserActiveTicketsAsync(userId);

            var ticketDTOs = _mapper.Map<List<TicketDTO>>(tickets);

            return View(ticketDTOs);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
