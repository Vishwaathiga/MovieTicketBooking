using MovieTicketBooking.DTOs.Seats;

namespace MovieTicketBooking.Services.Interfaces;

public interface ISeatService
{
    Task<List<SeatResponseDto>> GetByScreenIdAsync(int screenId);

    Task<SeatResponseDto?> GetByIdAsync(int id);

    Task<SeatResponseDto?> CreateAsync(CreateSeatDto dto);

    Task<List<SeatResponseDto>?> CreateBulkAsync(
        List<CreateSeatDto> dtos);

    Task<bool> UpdateAsync(int id, UpdateSeatDto dto);

    Task<bool> DeleteAsync(int id);
}