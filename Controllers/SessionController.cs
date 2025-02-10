using Cinema.Models.DataBaseModels;
using Cinema.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Cinema.DTOs;

namespace Cinema.Controllers
{
    public class SessionsController : Controller
    {
        private readonly UserManager<User> _userManager; // Додаємо UserManager
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;  // Додаємо AutoMapper

        // Вбудовуємо UserManager та AutoMapper через конструктор
        public SessionsController(UserManager<User> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;  // Інжекція AutoMapper
        }

        // 📌 Перегляд усіх сеансів
        public async Task<IActionResult> Sessions()
        {
            var sessions = await _unitOfWork.Sessions.GetAllSessionsAsync();
            var sessionDTOs = _mapper.Map<List<SessionDTO>>(sessions);  // Перетворюємо в DTO
            return View(sessionDTOs);  // Повертаємо список DTO
        }

        // 📌 Деталі сеансу
        public async Task<IActionResult> DetailsSession(Guid id)
        {
            var session = await _unitOfWork.Sessions.GetByIdSessionAsync(id);
            if (session == null) return NotFound();

            var movie = await _unitOfWork.Movies.GetByIdAsync(session.MovieId);
            var hall = await _unitOfWork.Halls.GetByIdAsync(session.HallId);

            var sessionDTO = _mapper.Map<SessionDTO>(session);
            sessionDTO.MovieTitle = movie?.Title;
            sessionDTO.HallName = hall?.Name;

            return View(sessionDTO);  // Передаємо DTO
        }


        // 📌 Форма створення сеансу
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSession()
        {
            ViewBag.Movies = new SelectList(await _unitOfWork.Movies.GetAllAsync(), "Id", "Title");
            ViewBag.Halls = new SelectList(await _unitOfWork.Halls.GetAllAsync(), "Id", "Name");
            return View();
        }

        // 📌 Додавання нового сеансу
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSession(SessionCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Movies = new SelectList(await _unitOfWork.Movies.GetAllAsync(), "Id", "Title");
                ViewBag.Halls = new SelectList(await _unitOfWork.Halls.GetAllAsync(), "Id", "Name");
                return View(model);
            }

            var session = _mapper.Map<Session>(model);  // Перетворюємо з ViewModel в модель

            await _unitOfWork.Sessions.AddAsync(session);
            await _unitOfWork.SaveAsync();

            // Отримуємо поточного користувача
            var currentUser = await _userManager.GetUserAsync(User);  // User - це поточний користувач в контексті аутентифікації
            var userId = currentUser?.Id;  // Отримуємо ID користувача, який увійшов

            if (userId == null)
            {
                // Якщо користувач не авторизований, можна обробити це випадок (наприклад, переадресувати на сторінку входу)
                return RedirectToAction("Login", "Account");
            }

            // 📌 Отримуємо всі місця в залі
            var seats = await _unitOfWork.Seats.GetSeatsByHallIdAsync(model.HallId);

            // 📌 Створюємо квитки для всіх місць
            foreach (var seat in seats)
            {
                var ticket = new Ticket
                {
                    Id = Guid.NewGuid(),
                    UserId = userId, // Присвоюємо ID користувача
                    SessionId = session.Id,
                    SeatId = seat.Id,
                    SeatNumber = seat.SeatNumber,
                    Status = "not sold",
                    Price = 0 // Початкова ціна
                };
                await _unitOfWork.Tickets.AddAsync(ticket);
            }

            await _unitOfWork.SaveAsync();

            // 📌 Перенаправляємо на сторінку керування квитками
            return RedirectToAction("ManageTickets", "Home", new { sessionId = session.Id });
        }

        // 📌 Форма редагування сеансу
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditSession(Guid id)
        {
            var session = await _unitOfWork.Sessions.GetByIdSessionAsync(id);

            if (session == null)
            {
                Console.WriteLine($"Session with ID {id} not found.");
                return NotFound();
            }

            Console.WriteLine($"MovieId: {session.MovieId}, HallId: {session.HallId}");

            var movies = await _unitOfWork.Movies.GetAllAsync();
            var halls = await _unitOfWork.Halls.GetAllAsync();

            if (movies == null || !movies.Any())
            {
                Console.WriteLine("Movies list is empty.");
            }

            if (halls == null || !halls.Any())
            {
                Console.WriteLine("Halls list is empty.");
            }

            ViewBag.Movies = new SelectList(await _unitOfWork.Movies.GetAllAsync(), "Id", "Title");
            ViewBag.Halls = new SelectList(await _unitOfWork.Halls.GetAllAsync(), "Id", "Name");

            return View(session);
        }

        // 📌 Оновлення сеансу
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditSession(Guid id, SessionDTO sessionDTO)
        {
            if (id != sessionDTO.Id) return NotFound();

            var session = _mapper.Map<Session>(sessionDTO);  // Перетворюємо DTO в модель
            await _unitOfWork.Sessions.UpdateAsync(session);
            await _unitOfWork.SaveAsync();
            return RedirectToAction("ManageTickets", "Home", new { sessionId = session.Id });
        }

        // 📌 Видалення сеансу
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSession(Guid id)
        {
            var session = await _unitOfWork.Sessions.GetByIdSessionAsync(id);
            if (session == null) return NotFound();
            return View(session);
        }

        // 📌 Підтвердження видалення
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("DeleteSession")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var session = await _unitOfWork.Sessions.GetByIdSessionAsync(id);
            if (session == null) return NotFound();

            await _unitOfWork.Sessions.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
            return RedirectToAction("Sessions", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> FilterSessions(double? minRating, DateTime? sessionDate)
        {
            var allSessions = await _unitOfWork.Sessions.GetAllSessionsAsync();
            var sessions = allSessions
                .Where(s =>
                    (!minRating.HasValue || s.Movie.Rating >= (decimal)minRating.Value) &&
                    (!sessionDate.HasValue || s.StartTime.Date == sessionDate.Value.Date))
                .ToList();

            var groupedSessions = sessions.GroupBy(s => s.Movie).ToList();

            return PartialView("_FilteredSessionsPartial", groupedSessions);
        }

    }
}
