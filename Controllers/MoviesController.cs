using Cinema.Models.DataBaseModels;
using Cinema.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cinema.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MoviesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Сторінка зі списком фільмів
        public async Task<IActionResult> Movies()
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();
            return View(movies);
        }

        // Сторінка для додавання нового фільму
        public IActionResult CreateMovie()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMovie(Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.Id = Guid.NewGuid(); // Генерація нового ID для фільму
                await _unitOfWork.Movies.AddAsync(movie);
                await _unitOfWork.SaveAsync();
                return RedirectToAction("Movies", "Home");
            }
            return View(movie);
        }

        // Сторінка для редагування фільму
        public async Task<IActionResult> EditMovie(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMovie(Guid id, Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _unitOfWork.Movies.UpdateAsync(movie);
                await _unitOfWork.SaveAsync();
                return RedirectToAction("Movies", "Home");
            }
            return View(movie);
        }

        // Видалення фільму
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        [HttpPost, ActionName("DeleteMovie")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie != null)
            {
                // Замість одночасного збереження, спочатку видаляйте, а потім збережіть
                await _unitOfWork.Movies.DeleteAsync(id);

                await _unitOfWork.SaveAsync();  // Викликається після завершення всіх операцій
            }
            return RedirectToAction("Movies", "Home"); // Повертає до списку фільмів
        }


        // Сторінка з деталями фільму
        public async Task<IActionResult> DetailsMovie(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

    }
}
