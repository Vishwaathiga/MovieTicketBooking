
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Bookings
{
    public class CancelBookingDto
    {
        [Required]
        public int BookingId { get; set; }
    }
}
