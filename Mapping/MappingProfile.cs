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
        }
    }
}
