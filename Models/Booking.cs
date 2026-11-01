
using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        [StringLength(20)]
        public string BookingReference { get; set; } = string.Empty;

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
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Confirmed";

        public decimal TotalAmount { get; set; }

        public Show? Show { get; set; }

        public ICollection<BookingSeat> BookingSeats { get; set; }
            = new List<BookingSeat>();
    }
}
