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

        public IActionResult Index()
        {
            return View();
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}