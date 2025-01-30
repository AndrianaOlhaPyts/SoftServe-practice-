using Cinema.Models.DataBaseModels;
using Cinema.Models.ViewModels;
using Cinema.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    public class SessionsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 📌 Перегляд усіх сеансів
        public async Task<IActionResult> Sessions()
        {
            var sessions = await _unitOfWork.Sessions.GetAllSessionsAsync();
            return View(sessions);
        }


        // 📌 Деталі сеансу
        public async Task<IActionResult> DetailsSession(Guid id)
        {
            var session = await _unitOfWork.Sessions.GetByIdSessionAsync(id);
            if (session == null) return NotFound();
            return View(session);
        }

        // 📌 Форма створення сеансу
        public async Task<IActionResult> CreateSession()
        {
            ViewBag.Movies = new SelectList(await _unitOfWork.Movies.GetAllAsync(), "Id", "Title");
            ViewBag.Halls = new SelectList(await _unitOfWork.Halls.GetAllAsync(), "Id", "Name");
            return View();
        }

        // 📌 Додавання нового сеансу
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSession(SessionCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }

                // Логування значень для перевірки
                Console.WriteLine($"MovieId: {model.MovieId}, HallId: {model.HallId}");

                ViewBag.Movies = new SelectList(await _unitOfWork.Movies.GetAllAsync(), "Id", "Title");
                ViewBag.Halls = new SelectList(await _unitOfWork.Halls.GetAllAsync(), "Id", "Name");

                return View(model);
            }

            // Перетворення ViewModel на модель Session
            
            var session = new Session
            {
                Id = Guid.NewGuid(),
                MovieId = model.MovieId,
                HallId = model.HallId,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };
            Console.WriteLine($"MovieId: {session.MovieId}, HallId: {session.HallId}");
            await _unitOfWork.Sessions.AddAsync(session);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("Sessions", "Home");
        }

        // 📌 Форма редагування сеансу
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

            ViewBag.Movies = new SelectList(movies, "Id", "Title", session.MovieId);
            ViewBag.Halls = new SelectList(halls, "Id", "Name", session.HallId);

            return View(session);
        }



        // 📌 Оновлення сеансу
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSession(Guid id, Session session)
        {
            

            if (id != session.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _unitOfWork.Sessions.UpdateAsync(session);
                await _unitOfWork.SaveAsync();
                return RedirectToAction("Sessions", "Home");
            }
            return View(session);
        }

        // 📌 Видалення сеансу
        public async Task<IActionResult> DeleteSession(Guid id)
        {
            var session = await _unitOfWork.Sessions.GetByIdSessionAsync(id);
            if (session == null) return NotFound();
            return View(session);
        }

        // 📌 Підтвердження видалення
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
    }
}
