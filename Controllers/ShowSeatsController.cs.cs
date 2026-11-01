
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowSeatsController : ControllerBase
    {
        private readonly IShowSeatRepository _showSeatRepository;

        public ShowSeatsController(
            IShowSeatRepository showSeatRepository)
        {
            _showSeatRepository = showSeatRepository;
        }

        [HttpGet("{showId}")]
        public async Task<IActionResult> GetByShowId(int showId)
        {
            var showSeats = await _showSeatRepository
                .GetByShowIdAsync(showId);

            if (showSeats.Count == 0)
            {
                return NotFound("No seats found for this show.");
            }

            return Ok(showSeats);
        }
    }
}
