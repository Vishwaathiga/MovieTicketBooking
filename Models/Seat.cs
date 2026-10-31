using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.Models;

public class Seat
{
    public int SeatId { get; set; }

    [Required]
    public int ScreenId { get; set; }

    [Required]
    [MaxLength(5)]
    public string RowName { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int SeatNumber { get; set; }

    [Required]
    [MaxLength(30)]
    public string SeatType { get; set; } = string.Empty;

    public Screen? Screen { get; set; }
}