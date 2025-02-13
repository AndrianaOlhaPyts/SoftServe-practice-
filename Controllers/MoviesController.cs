using AutoMapper;
using Cinema.Models.DataBaseModels;
using Cinema.DTOs;
using Cinema.Models.ViewModels;
using Cinema.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MoviesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Сторінка зі списком фільмів
        public async Task<IActionResult> Movies()
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();
            var movieDTOs = _mapper.Map<List<MovieDTO>>(movies);
            return View(movieDTOs);
        }

        // Сторінка для додавання нового фільму
        public IActionResult CreateMovie()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMovie(MovieDTO movieDTO)
        {
            if (ModelState.IsValid)
            {
                var movie = _mapper.Map<Movie>(movieDTO);
                movie.Id = Guid.NewGuid(); // Генерація нового ID
                await _unitOfWork.Movies.AddAsync(movie);
                await _unitOfWork.SaveAsync();
                return RedirectToAction("Movies","Home");
            }
            return View(movieDTO);
        }

        // Сторінка для редагування фільму
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditMovie(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            var movieDTO = _mapper.Map<MovieDTO>(movie);
            return View(movieDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMovie(Guid id, MovieDTO movieDTO)
        {
            if (id != movieDTO.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var movie = _mapper.Map<Movie>(movieDTO);
                await _unitOfWork.Movies.UpdateAsync(movie);
                await _unitOfWork.SaveAsync();
                return RedirectToAction("Movies","Home");
            }
            return View(movieDTO);
        }

        // Видалення фільму
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            var movieDTO = _mapper.Map<MovieDTO>(movie);
            return View(movieDTO);
        }

        [HttpPost, ActionName("DeleteMovie")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie != null)
            {
                await _unitOfWork.Movies.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
            }
            return RedirectToAction("Movies", "Home");
        }

        // Сторінка з деталями фільму
        public async Task<IActionResult> DetailsMovie(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            var sessions = await _unitOfWork.Sessions.GetSessionsByMovieIdAsync(id);

            var viewModel = new MovieDetailsViewModel
            {
                Movie = _mapper.Map<MovieDTO>(movie),
                Sessions = sessions
            };

            return View(viewModel);
        }
    }
}
