using System.ComponentModel.DataAnnotations;

namespace Cinema.Models.ViewModels
{
    public class SessionCreateViewModel
    {
        [Required]
        public Guid MovieId { get; set; }

        [Required]
        public Guid HallId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }

}