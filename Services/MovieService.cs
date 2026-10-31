using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<List<MovieResponseDto>> GetAllAsync(
        string? title,
        int pageNumber,
        int pageSize)
    {
        var movies = await _movieRepository.GetAllAsync(
            title,
            pageNumber,
            pageSize);

        return movies.Select(MapToResponse).ToList();
    }

    public async Task<MovieResponseDto?> GetByIdAsync(int id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);

        if (movie == null)
        {
            return null;
        }

        return MapToResponse(movie);
    }

    public async Task<MovieResponseDto?> GetByTitleAsync(string title)
    {
        var movie = await _movieRepository.GetByTitleAsync(title);

        if (movie == null)
        {
            return null;
        }

        return MapToResponse(movie);
    }

    public async Task<MovieResponseDto> CreateAsync(CreateMovieDto dto)
    {
        var movie = new Movie
        {
            Title = dto.Title,
            Description = dto.Description,
            DurationMinutes = dto.DurationMinutes,
            ReleaseDate = dto.ReleaseDate,
            Language = dto.Language,
            Genre = dto.Genre
        };

        await _movieRepository.AddAsync(movie);

        return MapToResponse(movie);
    }

    public async Task<bool> UpdateAsync(int id, UpdateMovieDto dto)
    {
        var movie = await _movieRepository.GetByIdAsync(id);

        if (movie == null)
        {
            return false;
        }

        movie.Title = dto.Title;
        movie.Description = dto.Description;
        movie.DurationMinutes = dto.DurationMinutes;
        movie.ReleaseDate = dto.ReleaseDate;
        movie.Language = dto.Language;
        movie.Genre = dto.Genre;

        await _movieRepository.UpdateAsync(movie);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);

        if (movie == null)
        {
            return false;
        }

        await _movieRepository.DeleteAsync(movie);

        return true;
    }

    private static MovieResponseDto MapToResponse(Movie movie)
    {
        return new MovieResponseDto
        {
            MovieId = movie.MovieId,
            Title = movie.Title,
            Description = movie.Description,
            DurationMinutes = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate,
            Language = movie.Language,
            Genre = movie.Genre
        };
    }
}