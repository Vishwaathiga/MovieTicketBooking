using MovieTicketBooking.DTOs.Movies;

namespace MovieTicketBooking.Services.Interfaces;

public interface IMovieService
{
    Task<List<MovieResponseDto>> GetAllAsync(
        string? title,
        int pageNumber,
        int pageSize);

    Task<MovieResponseDto?> GetByIdAsync(int id);

    Task<MovieResponseDto?> GetByTitleAsync(string title);

    Task<MovieResponseDto> CreateAsync(CreateMovieDto dto);

    Task<bool> UpdateAsync(int id, UpdateMovieDto dto);

    Task<bool> DeleteAsync(int id);
}