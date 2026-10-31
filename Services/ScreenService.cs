using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class ScreenService : IScreenService
{
    private readonly IScreenRepository _screenRepository;
    private readonly ITheatreRepository _theatreRepository;

    public ScreenService(
        IScreenRepository screenRepository,
        ITheatreRepository theatreRepository)
    {
        _screenRepository = screenRepository;
        _theatreRepository = theatreRepository;
    }

    public async Task<List<ScreenResponseDto>> GetAllAsync(int? theatreId)
    {
        var screens = await _screenRepository.GetAllAsync(theatreId);

        return screens.Select(MapToResponse).ToList();
    }

    public async Task<ScreenResponseDto?> GetByIdAsync(int id)
    {
        var screen = await _screenRepository.GetByIdAsync(id);

        if (screen == null)
        {
            return null;
        }

        return MapToResponse(screen);
    }

    public async Task<List<ScreenResponseDto>> GetByTheatreIdAsync(
        int theatreId)
    {
        var screens = await _screenRepository.GetByTheatreIdAsync(theatreId);

        return screens.Select(MapToResponse).ToList();
    }

    public async Task<ScreenResponseDto?> CreateAsync(
        CreateScreenDto dto)
    {
        var theatre = await _theatreRepository.GetByIdAsync(dto.TheatreId);

        if (theatre == null)
        {
            return null;
        }

        var exists = await _screenRepository.ExistsAsync(
            dto.TheatreId,
            dto.ScreenName);

        if (exists)
        {
            throw new InvalidOperationException(
                "A screen with this name already exists in the theatre.");
        }

        var screen = new Screen
        {
            TheatreId = dto.TheatreId,
            ScreenName = dto.ScreenName
        };

        await _screenRepository.AddAsync(screen);

        return MapToResponse(screen);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateScreenDto dto)
    {
        var screen = await _screenRepository.GetByIdAsync(id);

        if (screen == null)
        {
            return false;
        }

        var theatre = await _theatreRepository.GetByIdAsync(dto.TheatreId);

        if (theatre == null)
        {
            return false;
        }

        var exists = await _screenRepository.ExistsAsync(
            dto.TheatreId,
            dto.ScreenName);

        if (exists &&
            (screen.TheatreId != dto.TheatreId ||
             screen.ScreenName != dto.ScreenName))
        {
            throw new InvalidOperationException(
                "A screen with this name already exists in the theatre.");
        }

        screen.TheatreId = dto.TheatreId;
        screen.ScreenName = dto.ScreenName;

        await _screenRepository.UpdateAsync(screen);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var screen = await _screenRepository.GetByIdAsync(id);

        if (screen == null)
        {
            return false;
        }

        await _screenRepository.DeleteAsync(screen);

        return true;
    }

    private static ScreenResponseDto MapToResponse(Screen screen)
    {
        return new ScreenResponseDto
        {
            ScreenId = screen.ScreenId,
            TheatreId = screen.TheatreId,
            ScreenName = screen.ScreenName
        };
    }
}