
using Microsoft.AspNetCore.Http;

namespace MovieTicketBooking.Middleware
{
    public class AuthenticationDiagnosticsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationDiagnosticsMiddleware> _logger;

        public AuthenticationDiagnosticsMiddleware(
            RequestDelegate next,
            ILogger<AuthenticationDiagnosticsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("===== AUTHENTICATION DIAGNOSTICS =====");

            _logger.LogInformation(
                "Is Authenticated: {IsAuthenticated}",
                context.User.Identity?.IsAuthenticated);

            _logger.LogInformation(
                "Authentication Type: {AuthenticationType}",
                context.User.Identity?.AuthenticationType);

            _logger.LogInformation(
                "User Name: {UserName}",
                context.User.Identity?.Name);

            foreach (var claim in context.User.Claims)
            {
                _logger.LogInformation(
                    "Claim: {Type} = {Value}",
                    claim.Type,
                    claim.Value);
            }

            _logger.LogInformation("======================================");

            await _next(context);
        }
    }
}
