
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Constants;
using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[Route("api/screens")]
[ApiController]
public class ScreensController : ControllerBase
{
    private readonly IScreenService _screenService;

    public ScreensController(IScreenService screenService)
    {
        _screenService = screenService;
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<IActionResult> Create(CreateScreenDto dto)
    {
        try
        {
            var screen = await _screenService.CreateAsync(dto);

            if (screen == null)
            {
                return NotFound("Theatre not found.");
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = screen.ScreenId },
                screen);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? theatreId)
    {
        var screens = await _screenService.GetAllAsync(theatreId);

        return Ok(screens);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var screen = await _screenService.GetByIdAsync(id);

        if (screen == null)
        {
            return NotFound("Screen not found.");
        }

        return Ok(screen);
    }

    [HttpGet("theatre/{theatreId:int}")]
    public async Task<IActionResult> GetByTheatreId(int theatreId)
    {
        var screens = await _screenService.GetByTheatreIdAsync(theatreId);

        return Ok(screens);
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateScreenDto dto)
    {
        try
        {
            var updated = await _screenService.UpdateAsync(id, dto);

            if (!updated)
            {
                return NotFound("Screen or theatre not found.");
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _screenService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound("Screen not found.");
        }

        return NoContent();
    }
}
