using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _seatRepository;
    private readonly IScreenRepository _screenRepository;

    public SeatService(
        ISeatRepository seatRepository,
        IScreenRepository screenRepository)
    {
        _seatRepository = seatRepository;
        _screenRepository = screenRepository;
    }

    public async Task<List<SeatResponseDto>> GetByScreenIdAsync(
        int screenId)
    {
        var seats = await _seatRepository.GetByScreenIdAsync(screenId);

        return seats.Select(MapToResponse).ToList();
    }

    public async Task<SeatResponseDto?> GetByIdAsync(int id)
    {
        var seat = await _seatRepository.GetByIdAsync(id);

        if (seat == null)
        {
            return null;
        }

        return MapToResponse(seat);
    }

    public async Task<SeatResponseDto?> CreateAsync(CreateSeatDto dto)
    {
        var screen = await _screenRepository.GetByIdAsync(dto.ScreenId);

        if (screen == null)
        {
            return null;
        }

        var exists = await _seatRepository.ExistsAsync(
            dto.ScreenId,
            dto.RowName,
            dto.SeatNumber);

        if (exists)
        {
            throw new InvalidOperationException(
                "This seat already exists in the screen.");
        }

        var seat = new Seat
        {
            ScreenId = dto.ScreenId,
            RowName = dto.RowName,
            SeatNumber = dto.SeatNumber,
            SeatType = dto.SeatType
        };

        await _seatRepository.AddAsync(seat);

        return MapToResponse(seat);
    }

    public async Task<List<SeatResponseDto>?> CreateBulkAsync(
        List<CreateSeatDto> dtos)
    {
        if (dtos == null || dtos.Count == 0)
        {
            throw new InvalidOperationException(
                "At least one seat is required.");
        }

        var screenIds = dtos
            .Select(s => s.ScreenId)
            .Distinct()
            .ToList();

        if (screenIds.Count != 1)
        {
            throw new InvalidOperationException(
                "All seats must belong to the same screen.");
        }

        var screenId = screenIds[0];

        var screen = await _screenRepository.GetByIdAsync(screenId);

        if (screen == null)
        {
            return null;
        }

        var duplicateSeats = dtos
            .GroupBy(s => new
            {
                s.RowName,
                s.SeatNumber
            })
            .Any(g => g.Count() > 1);

        if (duplicateSeats)
        {
            throw new InvalidOperationException(
                "The request contains duplicate seat positions.");
        }

        foreach (var dto in dtos)
        {
            var exists = await _seatRepository.ExistsAsync(
                dto.ScreenId,
                dto.RowName,
                dto.SeatNumber);

            if (exists)
            {
                throw new InvalidOperationException(
                    $"Seat {dto.RowName}{dto.SeatNumber} already exists.");
            }
        }

        var seats = dtos.Select(dto => new Seat
        {
            ScreenId = dto.ScreenId,
            RowName = dto.RowName,
            SeatNumber = dto.SeatNumber,
            SeatType = dto.SeatType
        }).ToList();

        await _seatRepository.AddRangeAsync(seats);

        return seats.Select(MapToResponse).ToList();
    }

    public async Task<bool> UpdateAsync(int id, UpdateSeatDto dto)
    {
        var seat = await _seatRepository.GetByIdAsync(id);

        if (seat == null)
        {
            return false;
        }

        var screen = await _screenRepository.GetByIdAsync(dto.ScreenId);

        if (screen == null)
        {
            return false;
        }

        var exists = await _seatRepository.ExistsAsync(
            dto.ScreenId,
            dto.RowName,
            dto.SeatNumber);

        if (exists &&
            (seat.ScreenId != dto.ScreenId ||
             seat.RowName != dto.RowName ||
             seat.SeatNumber != dto.SeatNumber))
        {
            throw new InvalidOperationException(
                "This seat position already exists in the screen.");
        }

        seat.ScreenId = dto.ScreenId;
        seat.RowName = dto.RowName;
        seat.SeatNumber = dto.SeatNumber;
        seat.SeatType = dto.SeatType;

        await _seatRepository.UpdateAsync(seat);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var seat = await _seatRepository.GetByIdAsync(id);

        if (seat == null)
        {
            return false;
        }

        await _seatRepository.DeleteAsync(seat);

        return true;
    }

    private static SeatResponseDto MapToResponse(Seat seat)
    {
        return new SeatResponseDto
        {
            SeatId = seat.SeatId,
            ScreenId = seat.ScreenId,
            RowName = seat.RowName,
            SeatNumber = seat.SeatNumber,
            SeatType = seat.SeatType
        };
    }
}