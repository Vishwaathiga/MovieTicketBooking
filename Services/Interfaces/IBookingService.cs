
using MovieTicketBooking.DTOs.Bookings;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingResponseDto>> GetAllAsync();

        Task<BookingResponseDto?> GetByIdAsync(int bookingId);

        Task<BookingResponseDto?> GetByReferenceAsync(string bookingReference);

        Task<List<BookingResponseDto>> GetByCustomerEmailAsync(string customerEmail);

        Task<BookingResponseDto> CreateAsync(CreateBookingDto dto);

        Task<bool> CancelAsync(int bookingId);
    }
}
