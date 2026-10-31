using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class ScreenRepository : IScreenRepository
{
    private readonly ApplicationDbContext _context;

    public ScreenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Screen>> GetAllAsync(int? theatreId)
    {
        IQueryable<Screen> query = _context.Screens;

        if (theatreId.HasValue)
        {
            query = query.Where(s => s.TheatreId == theatreId.Value);
        }

        return await query
            .OrderBy(s => s.ScreenId)
            .ToListAsync();
    }

    public async Task<Screen?> GetByIdAsync(int id)
    {
        return await _context.Screens.FindAsync(id);
    }

    public async Task<List<Screen>> GetByTheatreIdAsync(int theatreId)
    {
        return await _context.Screens
            .Where(s => s.TheatreId == theatreId)
            .OrderBy(s => s.ScreenId)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(
        int theatreId,
        string screenName)
    {
        return await _context.Screens.AnyAsync(
            s => s.TheatreId == theatreId &&
                 s.ScreenName == screenName);
    }

    public async Task AddAsync(Screen screen)
    {
        await _context.Screens.AddAsync(screen);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Screen screen)
    {
        _context.Screens.Update(screen);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Screen screen)
    {
        _context.Screens.Remove(screen);
        await _context.SaveChangesAsync();
    }
}