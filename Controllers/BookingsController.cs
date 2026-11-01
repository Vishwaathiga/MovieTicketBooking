
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllAsync();

            return Ok(bookings);
        }

        // GET: api/bookings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            return Ok(booking);
        }

        // GET: api/bookings/reference/ABC123
        [HttpGet("reference/{bookingReference}")]
        public async Task<IActionResult> GetByReference(
            string bookingReference)
        {
            var booking = await _bookingService
                .GetByReferenceAsync(bookingReference);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            return Ok(booking);
        }

        // GET: api/bookings/customer/email@example.com
        [HttpGet("customer/{customerEmail}")]
        public async Task<IActionResult> GetByCustomerEmail(
            string customerEmail)
        {
            var bookings = await _bookingService
                .GetByCustomerEmailAsync(customerEmail);

            return Ok(bookings);
        }

        // POST: api/bookings
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateBookingDto dto)
        {
            try
            {
                var booking = await _bookingService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = booking.BookingId },
                    booking);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // PUT: api/bookings/5/cancel
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var result = await _bookingService.CancelAsync(id);

                if (!result)
                {
                    return NotFound("Booking not found.");
                }

                return Ok("Booking cancelled successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
