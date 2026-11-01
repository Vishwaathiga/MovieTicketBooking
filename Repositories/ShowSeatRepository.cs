
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class ShowSeatRepository : IShowSeatRepository
    {
        private readonly ApplicationDbContext _context;

        public ShowSeatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShowSeat>> GetByShowIdAsync(int showId)
        {
            return await _context.ShowSeats
                .Include(ss => ss.Seat)
                .Where(ss => ss.ShowId == showId)
                .ToListAsync();
        }

        public async Task AddRangeAsync(List<ShowSeat> showSeats)
        {
            await _context.ShowSeats.AddRangeAsync(showSeats);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(List<ShowSeat> showSeats)
        {
            _context.ShowSeats.UpdateRange(showSeats);
            await _context.SaveChangesAsync();
        }
    }
}
