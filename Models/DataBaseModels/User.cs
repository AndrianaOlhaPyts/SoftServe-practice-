using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Cinema.Models.DataBaseModels
{
    public class User : IdentityUser
    {
        // Навігаційні властивості
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
