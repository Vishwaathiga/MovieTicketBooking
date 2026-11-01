
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllAsync();

        Task<Booking?> GetByIdAsync(int bookingId);

        Task<Booking?> GetByReferenceAsync(string bookingReference);

        Task<List<Booking>> GetByCustomerEmailAsync(string customerEmail);

        Task AddAsync(Booking booking);

        Task UpdateAsync(Booking booking);
    }
}
