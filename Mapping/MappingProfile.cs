using AutoMapper;
using Cinema.DTOs;
using Cinema.Models.DataBaseModels;

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
        }
    }
}
