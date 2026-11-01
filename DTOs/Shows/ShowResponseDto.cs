
namespace MovieTicketBooking.DTOs.Shows
{
    public class ShowResponseDto
    {
        public int ShowId { get; set; }

        public int MovieId { get; set; }

        public string? MovieTitle { get; set; }

        public int ScreenId { get; set; }

        public string? ScreenName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal TicketPrice { get; set; }
    }
}
