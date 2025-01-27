using Cinema.Models.DataBaseModels;
using Cinema.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // Управління фільмами
        public async Task<IActionResult> Movies()
        {
            var movies = await _adminService.GetAllMoviesAsync();
            return View(movies);
        }

        public async Task<IActionResult> MovieDetails(Guid id)
        {
            var movie = await _adminService.GetMovieByIdAsync(id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(Movie movie)
        {
            if (!ModelState.IsValid) return View(movie);
            await _adminService.CreateMovieAsync(movie);
            return RedirectToAction(nameof(Movies));
        }

        // Управління сеансами
        public async Task<IActionResult> Sessions()
        {
            var sessions = await _adminService.GetAllSessionsAsync();
            return View(sessions);
        }

        public async Task<IActionResult> SessionDetails(Guid id)
        {
            var session = await _adminService.GetSessionByIdAsync(id);
            if (session == null) return NotFound();
            return View(session);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession(Session session)
        {
            if (!ModelState.IsValid) return View(session);
            await _adminService.CreateSessionAsync(session);
            return RedirectToAction(nameof(Sessions));
        }
    }
}
