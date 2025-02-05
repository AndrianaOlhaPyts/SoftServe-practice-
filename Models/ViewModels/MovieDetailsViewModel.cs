using Cinema.DTOs;
using Cinema.Models.DataBaseModels;

namespace Cinema.Models.ViewModels
{
    internal class MovieDetailsViewModel
    {
        public MovieDTO Movie { get; set; }
        public IEnumerable<Session> Sessions { get; set; }
    }
}