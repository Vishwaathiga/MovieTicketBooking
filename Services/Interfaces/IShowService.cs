
using MovieTicketBooking.DTOs.Shows;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IShowService
    {
        Task<List<ShowResponseDto>> GetAllAsync();

        Task<ShowResponseDto?> GetByIdAsync(int id);

        Task<List<ShowResponseDto>> GetByMovieIdAsync(int movieId);

        Task<List<ShowResponseDto>> GetByScreenIdAsync(int screenId);

        Task<ShowResponseDto> CreateAsync(CreateShowDto dto);

        Task<ShowResponseDto?> UpdateAsync(int id, UpdateShowDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
