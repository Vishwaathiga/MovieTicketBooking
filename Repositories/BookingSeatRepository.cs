
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class BookingSeatRepository : IBookingSeatRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingSeatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookingSeat>> GetByBookingIdAsync(int bookingId)
        {
            return await _context.BookingSeats
                .Include(bs => bs.ShowSeat)
                    .ThenInclude(ss => ss!.Seat)
                .Where(bs => bs.BookingId == bookingId)
                .ToListAsync();
        }

        public async Task AddRangeAsync(List<BookingSeat> bookingSeats)
        {
            await _context.BookingSeats.AddRangeAsync(bookingSeats);
            await _context.SaveChangesAsync();
        }
    }
}
