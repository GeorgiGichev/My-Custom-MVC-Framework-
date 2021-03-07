using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedTrip.Models
{
    public class UserTrip
    {
        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public string TripId { get; set; }

        public Trip Trip { get; set; }
    }
}
