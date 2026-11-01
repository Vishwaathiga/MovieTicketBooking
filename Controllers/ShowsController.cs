
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shows = await _showService.GetAllAsync();

            return Ok(shows);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var show = await _showService.GetByIdAsync(id);

            if (show == null)
            {
                return NotFound("Show not found.");
            }

            return Ok(show);
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetByMovieId(int movieId)
        {
            var shows = await _showService.GetByMovieIdAsync(movieId);

            return Ok(shows);
        }

        [HttpGet("screen/{screenId}")]
        public async Task<IActionResult> GetByScreenId(int screenId)
        {
            var shows = await _showService.GetByScreenIdAsync(screenId);

            return Ok(shows);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateShowDto dto)
        {
            try
            {
                var show = await _showService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = show.ShowId },
                    show);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateShowDto dto)
        {
            try
            {
                var show = await _showService.UpdateAsync(id, dto);

                if (show == null)
                {
                    return NotFound("Show not found.");
                }

                return Ok(show);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _showService.DeleteAsync(id);

                if (!result)
                {
                    return NotFound("Show not found.");
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
