
namespace MovieTicketBooking.DTOs.Bookings
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }

        public string BookingReference { get; set; } = string.Empty;

        public int ShowId { get; set; }

        public string MovieTitle { get; set; } = string.Empty;

        public string TheatreName { get; set; } = string.Empty;

        public string ScreenName { get; set; } = string.Empty;

        public DateTime ShowStartTime { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public DateTime BookingDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public List<string> SeatNames { get; set; } = new List<string>();
    }
}
