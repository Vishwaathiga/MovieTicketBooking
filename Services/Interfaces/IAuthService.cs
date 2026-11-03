
using MovieTicketBooking.DTOs;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDTO registerDTO);

        Task<string> LoginAsync(LoginDTO loginDTO);
    }
}
