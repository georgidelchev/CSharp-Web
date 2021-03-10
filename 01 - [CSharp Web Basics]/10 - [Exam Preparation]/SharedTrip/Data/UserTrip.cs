using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Data
{
    public class UserTrip
    {
        [Required]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Required]
        public string TripId { get; set; }

        public virtual Trip Trip { get; set; }
    }
}