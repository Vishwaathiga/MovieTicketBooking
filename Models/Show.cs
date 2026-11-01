
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTicketBooking.Models
{
    public class Show
    {
        public int ShowId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int ScreenId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 99999999.99)]
        public decimal TicketPrice { get; set; }

        public Movie? Movie { get; set; }

        public Screen? Screen { get; set; }

        public ICollection<ShowSeat> ShowSeats { get; set; } = new List<ShowSeat>();
    }
}
