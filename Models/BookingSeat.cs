
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBooking.Models
{
    public class BookingSeat
    {
        public int BookingSeatId { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        public int ShowSeatId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public Booking? Booking { get; set; }

        public ShowSeat? ShowSeat { get; set; }
    }
}
