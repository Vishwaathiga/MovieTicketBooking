using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class TheatreService : ITheatreService
{
    private readonly ITheatreRepository _theatreRepository;

    public TheatreService(ITheatreRepository theatreRepository)
    {
        _theatreRepository = theatreRepository;
    }

    public async Task<List<TheatreResponseDto>> GetAllAsync()
    {
        var theatres = await _theatreRepository.GetAllAsync();

        return theatres.Select(MapToResponse).ToList();
    }

    public async Task<TheatreResponseDto?> GetByIdAsync(int id)
    {
        var theatre = await _theatreRepository.GetByIdAsync(id);

        if (theatre == null)
        {
            return null;
        }

        return MapToResponse(theatre);
    }

    public async Task<TheatreResponseDto> CreateAsync(
        CreateTheatreDto dto)
    {
        var theatre = new Theatre
        {
            Name = dto.Name,
            Location = dto.Location
        };

        await _theatreRepository.AddAsync(theatre);

        return MapToResponse(theatre);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateTheatreDto dto)
    {
        var theatre = await _theatreRepository.GetByIdAsync(id);

        if (theatre == null)
        {
            return false;
        }

        theatre.Name = dto.Name;
        theatre.Location = dto.Location;

        await _theatreRepository.UpdateAsync(theatre);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var theatre = await _theatreRepository.GetByIdAsync(id);

        if (theatre == null)
        {
            return false;
        }

        await _theatreRepository.DeleteAsync(theatre);

        return true;
    }

    private static TheatreResponseDto MapToResponse(Theatre theatre)
    {
        return new TheatreResponseDto
        {
            TheatreId = theatre.TheatreId,
            Name = theatre.Name,
            Location = theatre.Location
        };
    }
}