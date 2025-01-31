using Cinema.Models.DataBaseModels;

namespace Cinema.Models.ViewModels
{
    internal class MovieDetailsViewModel
    {
        public Movie Movie { get; set; }
        public IEnumerable<Session> Sessions { get; set; }
    }
}