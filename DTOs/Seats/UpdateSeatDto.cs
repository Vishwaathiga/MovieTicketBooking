using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Seats;

public class UpdateSeatDto
{
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
}