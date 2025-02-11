using AutoMapper;
using Cinema.DTOs;
using Cinema.Models.DataBaseModels;
using Cinema.Models.ViewModels;

namespace Cinema.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hall, HallDTO>();
            CreateMap<Movie, MovieDTO>();
            CreateMap<Row, RowDTO>();
            CreateMap<Seat, SeatDTO>();
            CreateMap<Session, SessionDTO>();
            CreateMap<SalesStatistics, SalesStatisticsDTO>();
            CreateMap<Ticket, TicketDTO>();

            // Додаємо конфігурацію для перетворення SessionCreateViewModel в Session
            CreateMap<SessionCreateViewModel, Session>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))  // Якщо у вас є властивість MovieId
                .ForMember(dest => dest.HallId, opt => opt.MapFrom(src => src.HallId))  // Якщо у вас є властивість HallId
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime)); // Інші властивості
            // Перетворення з MovieDTO на Movie
            CreateMap<MovieDTO, Movie>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                // додайте інші поля, якщо потрібно
                ;
            CreateMap<SessionDTO, Session>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.HallId, opt => opt.MapFrom(src => src.HallId))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));

            // Перетворення з Ticket до TicketDTO
            CreateMap<Ticket, TicketDTO>()
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.Seat.SeatNumber)) // Приклад перетворення для Seat
                .ForMember(dest => dest.SeatId, opt => opt.MapFrom(src => src.SeatId)) // Перетворення SeatId
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)) // Статус квитка
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price)); // Ціна квитка

            // Перетворення з TicketDTO на Ticket
            CreateMap<TicketDTO, Ticket>()
                .ForMember(dest => dest.SeatId, opt => opt.MapFrom(src => src.SeatId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
            CreateMap<SalesStatisticsDTO, SalesStatistics>()
                .ForMember(dest => dest.TicketsSold, opt => opt.MapFrom(src => src.TicketsSold))
                .ForMember(dest => dest.Revenue, opt => opt.MapFrom(src => src.Revenue));

            CreateMap<SalesStatistics, SalesStatisticsDTO>()
                .ForMember(dest => dest.TicketsSold, opt => opt.MapFrom(src => src.TicketsSold))
                .ForMember(dest => dest.Revenue, opt => opt.MapFrom(src => src.Revenue));
            CreateMap<Seat, SeatDTO>()
                .ForMember(dest => dest.Row, opt => opt.MapFrom(src => src.Row));
        }
    }
}
