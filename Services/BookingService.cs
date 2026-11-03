
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingSeatRepository _bookingSeatRepository;
        private readonly IShowRepository _showRepository;
        private readonly IShowSeatRepository _showSeatRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IBookingSeatRepository bookingSeatRepository,
            IShowRepository showRepository,
            IShowSeatRepository showSeatRepository)
        {
            _bookingRepository = bookingRepository;
            _bookingSeatRepository = bookingSeatRepository;
            _showRepository = showRepository;
            _showSeatRepository = showSeatRepository;
        }

        public async Task<List<BookingResponseDto>> GetAllAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();

            return bookings.Select(MapToDto).ToList();
        }

        public async Task<BookingResponseDto?> GetByIdAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return null;
            }

            return MapToDto(booking);
        }

        public async Task<BookingResponseDto?> GetByReferenceAsync(
            string bookingReference)
        {
            var booking = await _bookingRepository
                .GetByReferenceAsync(bookingReference);

            if (booking == null)
            {
                return null;
            }

            return MapToDto(booking);
        }

        public async Task<List<BookingResponseDto>> GetByCustomerEmailAsync(
            string customerEmail)
        {
            var bookings = await _bookingRepository
                .GetByCustomerEmailAsync(customerEmail);

            return bookings.Select(MapToDto).ToList();
        }

        public async Task<BookingResponseDto> CreateAsync(
            CreateBookingDto dto)
        {
            if (dto.ShowSeatIds == null || dto.ShowSeatIds.Count == 0)
            {
                throw new InvalidOperationException(
                    "At least one seat must be selected.");
            }

            if (dto.ShowSeatIds.Distinct().Count() != dto.ShowSeatIds.Count)
            {
                throw new InvalidOperationException(
                    "Duplicate seat selections are not allowed.");
            }

            var show = await _showRepository.GetByIdAsync(dto.ShowId);

            if (show == null)
            {
                throw new InvalidOperationException("Show not found.");
            }

            var showSeats = await _showSeatRepository
                .GetByShowIdAsync(dto.ShowId);

            var selectedShowSeats = showSeats
                .Where(ss => dto.ShowSeatIds.Contains(ss.ShowSeatId))
                .ToList();
            Console.WriteLine($"Requested seats: {dto.ShowSeatIds.Count}");
            Console.WriteLine($"Selected seats found: {selectedShowSeats.Count}");
            Console.WriteLine($"Booking total: {show.TicketPrice * selectedShowSeats.Count}");

            if (selectedShowSeats.Count != dto.ShowSeatIds.Count)
            {
                throw new InvalidOperationException(
                    "One or more selected seats do not belong to this show.");
            }

            if (selectedShowSeats.Any(ss => ss.Status != "Available"))
            {
                throw new InvalidOperationException(
                    "One or more selected seats are already booked.");
            }

            var totalAmount = show.TicketPrice * selectedShowSeats.Count;

            var booking = new Booking
            {
                BookingReference = Guid.NewGuid()
                    .ToString("N")
                    .Substring(0, 12)
                    .ToUpper(),

                ShowId = dto.ShowId,
                CustomerName = dto.CustomerName,
                CustomerEmail = dto.CustomerEmail,
                BookingDate = DateTime.Now,
                Status = "Confirmed",
                TotalAmount = totalAmount
            };

            await _bookingRepository.AddAsync(booking);

            var bookingSeats = selectedShowSeats
                .Select(ss => new BookingSeat
                {
                    BookingId = booking.BookingId,
                    ShowSeatId = ss.ShowSeatId,
                    Price = show.TicketPrice
                })
                .ToList();

            await _bookingSeatRepository.AddRangeAsync(bookingSeats);

            foreach (var showSeat in selectedShowSeats)
            {
                showSeat.Status = "Booked";
            }

            await _showSeatRepository
                .UpdateRangeAsync(selectedShowSeats);

            var createdBooking = await _bookingRepository
                .GetByIdAsync(booking.BookingId);

            if (createdBooking == null)
            {
                throw new InvalidOperationException(
                    "Unable to retrieve the created booking.");
            }

            return MapToDto(createdBooking);
        }

        public async Task<bool> CancelAsync(int bookingId)
        {
            var booking = await _bookingRepository
                .GetByIdAsync(bookingId);

            if (booking == null)
            {
                return false;
            }

            if (booking.Status == "Cancelled")
            {
                throw new InvalidOperationException(
                    "This booking is already cancelled.");
            }

            booking.Status = "Cancelled";

            await _bookingRepository.UpdateAsync(booking);

            var bookingSeats = await _bookingSeatRepository
                .GetByBookingIdAsync(bookingId);

            var showSeats = await _showSeatRepository
                .GetByShowIdAsync(booking.ShowId);

            foreach (var bookingSeat in bookingSeats)
            {
                var showSeat = showSeats.FirstOrDefault(
                    ss => ss.ShowSeatId == bookingSeat.ShowSeatId);

                if (showSeat != null)
                {
                    showSeat.Status = "Available";
                }
            }

            await _showSeatRepository
                .UpdateRangeAsync(showSeats);

            return true;
        }

        private static BookingResponseDto MapToDto(Booking booking)
        {
            return new BookingResponseDto
            {
                BookingId = booking.BookingId,
                BookingReference = booking.BookingReference,
                ShowId = booking.ShowId,

                MovieTitle = booking.Show?.Movie?.Title
                    ?? string.Empty,

                TheatreName = booking.Show?.Screen?.Theatre?.Name
                    ?? string.Empty,

                ScreenName = booking.Show?.Screen?.ScreenName
                    ?? string.Empty,

                ShowStartTime = booking.Show?.StartTime
                    ?? DateTime.MinValue,

                CustomerName = booking.CustomerName,
                CustomerEmail = booking.CustomerEmail,
                BookingDate = booking.BookingDate,
                Status = booking.Status,
                TotalAmount = booking.TotalAmount,

                SeatNames = booking.BookingSeats
                    .Select(bs =>
                        $"{bs.ShowSeat?.Seat?.RowName}{bs.ShowSeat?.Seat?.SeatNumber}")
                    .ToList()
            };
        }
    }
}
