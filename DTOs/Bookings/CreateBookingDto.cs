
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Bookings
{
    public class CreateBookingDto
    {
        [Required]
        public int ShowId { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        public List<int> ShowSeatIds { get; set; } = new List<int>();
    }
}
