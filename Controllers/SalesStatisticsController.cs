using AutoMapper;
using Cinema.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Cinema.Models.DataBaseModels;

namespace Cinema.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SalesStatisticsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SalesStatisticsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // 📌 Відображення завершених сеансів
        public async Task<IActionResult> CompletedSessions()
        {
            var completedSessions = (await _unitOfWork.Sessions.GetAllSessionsAsync())
                .Where(s => s.EndTime <= DateTime.Now);
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
                    .GetAllAsync(t => t.SessionId == sessionId && t.Status == "paid");

                var newStats = new SalesStatisticsDTO
                {
                    Id = Guid.NewGuid(),
                    SessionId = sessionId,
                    TicketsSold = tickets.Count(),
                    Revenue = tickets.Sum(t => t.Price)
                };

                // Мапінг DTO на модель перед збереженням у БД
                var salesStatsEntity = _mapper.Map<SalesStatistics>(newStats);
                await _unitOfWork.SalesStatistics.AddAsync(salesStatsEntity);
                await _unitOfWork.SaveAsync();

                existingStats = salesStatsEntity;
            }

            // Мапимо `SalesStatistics` на `SalesStatisticsDTO` перед відображенням
            var statsDTO = _mapper.Map<SalesStatisticsDTO>(existingStats);
            return View(statsDTO);
        }
    }
}
