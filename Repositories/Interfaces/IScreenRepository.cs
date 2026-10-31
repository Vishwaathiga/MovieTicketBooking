using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IScreenRepository
{
    Task<List<Screen>> GetAllAsync(int? theatreId);

    Task<Screen?> GetByIdAsync(int id);

    Task<List<Screen>> GetByTheatreIdAsync(int theatreId);

    Task<bool> ExistsAsync(int theatreId, string screenName);

    Task AddAsync(Screen screen);

    Task UpdateAsync(Screen screen);

    Task DeleteAsync(Screen screen);
}