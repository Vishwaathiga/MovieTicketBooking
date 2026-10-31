using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Screens;

public class CreateScreenDto
{
    [Required]
    public int TheatreId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ScreenName { get; set; } = string.Empty;
}