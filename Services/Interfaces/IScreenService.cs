using MovieTicketBooking.DTOs.Screens;

namespace MovieTicketBooking.Services.Interfaces;

public interface IScreenService
{
    Task<List<ScreenResponseDto>> GetAllAsync(int? theatreId);

    Task<ScreenResponseDto?> GetByIdAsync(int id);

    Task<List<ScreenResponseDto>> GetByTheatreIdAsync(int theatreId);

    Task<ScreenResponseDto?> CreateAsync(CreateScreenDto dto);

    Task<bool> UpdateAsync(int id, UpdateScreenDto dto);

    Task<bool> DeleteAsync(int id);
}