
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IShowRepository
    {
        Task<List<Show>> GetAllAsync();

        Task<Show?> GetByIdAsync(int id);

        Task<List<Show>> GetByMovieIdAsync(int movieId);

        Task<List<Show>> GetByScreenIdAsync(int screenId);

        Task<bool> HasOverlapAsync(
            int screenId,
            DateTime startTime,
            DateTime endTime,
            int? excludeShowId = null);

        Task AddAsync(Show show);

        Task UpdateAsync(Show show);

        Task DeleteAsync(Show show);
    }
}
