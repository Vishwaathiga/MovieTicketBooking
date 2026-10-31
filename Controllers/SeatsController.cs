using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[Route("api/seats")]
[ApiController]
public class SeatsController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatsController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSeatDto dto)
    {
        try
        {
            var seat = await _seatService.CreateAsync(dto);

            if (seat == null)
            {
                return NotFound("Screen not found.");
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = seat.SeatId },
                seat);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> CreateBulk(
        List<CreateSeatDto> dtos)
    {
        try
        {
            var seats = await _seatService.CreateBulkAsync(dtos);

            if (seats == null)
            {
                return NotFound("Screen not found.");
            }

            return Ok(seats);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpGet("screen/{screenId:int}")]
    public async Task<IActionResult> GetByScreenId(int screenId)
    {
        var seats = await _seatService.GetByScreenIdAsync(screenId);

        return Ok(seats);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var seat = await _seatService.GetByIdAsync(id);

        if (seat == null)
        {
            return NotFound("Seat not found.");
        }

        return Ok(seat);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateSeatDto dto)
    {
        try
        {
            var updated = await _seatService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound("Seat or screen not found.");
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _seatService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound("Seat not found.");
        }

        return NoContent();
    }
}