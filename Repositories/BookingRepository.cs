
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Movie)
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Screen)
                        .ThenInclude(s => s!.Theatre)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.ShowSeat)
                        .ThenInclude(ss => ss!.Seat)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Movie)
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Screen)
                        .ThenInclude(s => s!.Theatre)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.ShowSeat)
                        .ThenInclude(ss => ss!.Seat)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<Booking?> GetByReferenceAsync(string bookingReference)
        {
            return await _context.Bookings
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Movie)
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Screen)
                        .ThenInclude(s => s!.Theatre)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.ShowSeat)
                        .ThenInclude(ss => ss!.Seat)
                .FirstOrDefaultAsync(b => b.BookingReference == bookingReference);
        }

        public async Task<List<Booking>> GetByCustomerEmailAsync(string customerEmail)
        {
            return await _context.Bookings
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Movie)
                .Include(b => b.Show)
                    .ThenInclude(s => s!.Screen)
                        .ThenInclude(s => s!.Theatre)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.ShowSeat)
                        .ThenInclude(ss => ss!.Seat)
                .Where(b => b.CustomerEmail == customerEmail)
                .ToListAsync();
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
    }
}
