
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Authorization.Requirements;

namespace MovieTicketBooking.Authorization
{
    public class BookingOwnerHandler
        : AuthorizationHandler<BookingOwnerRequirement, BookingResponseDto>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            BookingOwnerRequirement requirement,
            BookingResponseDto booking)
        {
            var userEmail = context.User.FindFirstValue(ClaimTypes.Email)
                ?? context.User.FindFirstValue("email")
                ?? context.User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");

            Console.WriteLine("===== AUTHORIZATION DEBUG =====");
            Console.WriteLine("Is Authenticated: " + context.User.Identity?.IsAuthenticated);
            Console.WriteLine("User Email: " + userEmail);
            Console.WriteLine("Booking Email: " + booking.CustomerEmail);
            Console.WriteLine("===============================");

            if (string.Equals(
                userEmail,
                booking.CustomerEmail,
                StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
                Console.WriteLine("Authorization: SUCCESS");
            }
            else
            {
                Console.WriteLine("Authorization: FAILED");
            }

            return Task.CompletedTask;
        }
    }
}
