using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllAsync(string? title, int pageNumber, int pageSize);

    Task<Movie?> GetByIdAsync(int id);

    Task<Movie?> GetByTitleAsync(string title);

    Task AddAsync(Movie movie);

    Task UpdateAsync(Movie movie);

    Task DeleteAsync(Movie movie);
}