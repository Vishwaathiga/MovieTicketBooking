
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;
        private readonly IShowSeatRepository _showSeatRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IScreenRepository _screenRepository;
        private readonly ISeatRepository _seatRepository;

        public ShowService(
            IShowRepository showRepository,
            IShowSeatRepository showSeatRepository,
            IMovieRepository movieRepository,
            IScreenRepository screenRepository,
            ISeatRepository seatRepository)
        {
            _showRepository = showRepository;
            _showSeatRepository = showSeatRepository;
            _movieRepository = movieRepository;
            _screenRepository = screenRepository;
            _seatRepository = seatRepository;
        }

        public async Task<List<ShowResponseDto>> GetAllAsync()
        {
            var shows = await _showRepository.GetAllAsync();

            return shows.Select(MapToResponse).ToList();
        }

        public async Task<ShowResponseDto?> GetByIdAsync(int id)
        {
            var show = await _showRepository.GetByIdAsync(id);

            if (show == null)
            {
                return null;
            }

            return MapToResponse(show);
        }

        public async Task<List<ShowResponseDto>> GetByMovieIdAsync(int movieId)
        {
            var shows = await _showRepository.GetByMovieIdAsync(movieId);

            return shows.Select(MapToResponse).ToList();
        }

        public async Task<List<ShowResponseDto>> GetByScreenIdAsync(int screenId)
        {
            var shows = await _showRepository.GetByScreenIdAsync(screenId);

            return shows.Select(MapToResponse).ToList();
        }

        public async Task<ShowResponseDto> CreateAsync(CreateShowDto dto)
        {
            if (dto.EndTime <= dto.StartTime)
            {
                throw new InvalidOperationException(
                    "End time must be later than start time.");
            }

            var movie = await _movieRepository.GetByIdAsync(dto.MovieId);

            if (movie == null)
            {
                throw new InvalidOperationException("Movie not found.");
            }

            var screen = await _screenRepository.GetByIdAsync(dto.ScreenId);

            if (screen == null)
            {
                throw new InvalidOperationException("Screen not found.");
            }

            var hasOverlap = await _showRepository.HasOverlapAsync(
                dto.ScreenId,
                dto.StartTime,
                dto.EndTime);

            if (hasOverlap)
            {
                throw new InvalidOperationException(
                    "Another show is already scheduled on this screen during that time.");
            }

            var seats = await _seatRepository.GetByScreenIdAsync(dto.ScreenId);

            if (seats.Count == 0)
            {
                throw new InvalidOperationException(
                    "No seats are configured for this screen.");
            }

            var show = new Show
            {
                MovieId = dto.MovieId,
                ScreenId = dto.ScreenId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                TicketPrice = dto.TicketPrice
            };

            await _showRepository.AddAsync(show);

            var showSeats = seats.Select(seat => new ShowSeat
            {
                ShowId = show.ShowId,
                SeatId = seat.SeatId,
                Status = "Available"
            }).ToList();

            await _showSeatRepository.AddRangeAsync(showSeats);

            var createdShow = await _showRepository.GetByIdAsync(show.ShowId);

            return MapToResponse(createdShow!);
        }

        public async Task<ShowResponseDto?> UpdateAsync(
            int id,
            UpdateShowDto dto)
        {
            var show = await _showRepository.GetByIdAsync(id);

            if (show == null)
            {
                return null;
            }

            if (dto.EndTime <= dto.StartTime)
            {
                throw new InvalidOperationException(
                    "End time must be later than start time.");
            }

            var movie = await _movieRepository.GetByIdAsync(dto.MovieId);

            if (movie == null)
            {
                throw new InvalidOperationException("Movie not found.");
            }

            var screen = await _screenRepository.GetByIdAsync(dto.ScreenId);

            if (screen == null)
            {
                throw new InvalidOperationException("Screen not found.");
            }

            if (show.ScreenId != dto.ScreenId)
            {
                throw new InvalidOperationException(
                    "The screen cannot be changed after a show is created.");
            }

            var hasOverlap = await _showRepository.HasOverlapAsync(
                dto.ScreenId,
                dto.StartTime,
                dto.EndTime,
                id);

            if (hasOverlap)
            {
                throw new InvalidOperationException(
                    "Another show is already scheduled on this screen during that time.");
            }

            show.MovieId = dto.MovieId;
            show.StartTime = dto.StartTime;
            show.EndTime = dto.EndTime;
            show.TicketPrice = dto.TicketPrice;

            await _showRepository.UpdateAsync(show);

            var updatedShow = await _showRepository.GetByIdAsync(id);

            return MapToResponse(updatedShow!);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var show = await _showRepository.GetByIdAsync(id);

            if (show == null)
            {
                return false;
            }

            var showSeats = await _showSeatRepository.GetByShowIdAsync(id);

            if (showSeats.Count > 0)
            {
                throw new InvalidOperationException(
                    "This show has seat records and cannot be deleted.");
            }

            await _showRepository.DeleteAsync(show);

            return true;
        }

        private static ShowResponseDto MapToResponse(Show show)
        {
            return new ShowResponseDto
            {
                ShowId = show.ShowId,
                MovieId = show.MovieId,
                MovieTitle = show.Movie?.Title,
                ScreenId = show.ScreenId,
                ScreenName = show.Screen?.ScreenName,
                StartTime = show.StartTime,
                EndTime = show.EndTime,
                TicketPrice = show.TicketPrice
            };
        }
    }
}
