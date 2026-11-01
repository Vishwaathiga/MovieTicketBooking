
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IBookingSeatRepository
    {
        Task<List<BookingSeat>> GetByBookingIdAsync(int bookingId);

        Task AddRangeAsync(List<BookingSeat> bookingSeats);
    }
}
