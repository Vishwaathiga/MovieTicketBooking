using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[Route("api/theatres")]
[ApiController]
public class TheatresController : ControllerBase
{
    private readonly ITheatreService _theatreService;

    public TheatresController(ITheatreService theatreService)
    {
        _theatreService = theatreService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTheatreDto dto)
    {
        var theatre = await _theatreService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = theatre.TheatreId },
            theatre);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var theatres = await _theatreService.GetAllAsync();

        return Ok(theatres);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var theatre = await _theatreService.GetByIdAsync(id);

        if (theatre == null)
        {
            return NotFound("Theatre not found.");
        }

        return Ok(theatre);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateTheatreDto dto)
    {
        var updated = await _theatreService.UpdateAsync(id, dto);

        if (!updated)
        {
            return NotFound("Theatre not found.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _theatreService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound("Theatre not found.");
        }

        return NoContent();
    }
}