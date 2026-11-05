
using Microsoft.AspNetCore.Http;

namespace MovieTicketBooking.Middleware
{
    public class AuthorizationDiagnosticsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthorizationDiagnosticsMiddleware> _logger;

        public AuthorizationDiagnosticsMiddleware(
            RequestDelegate next,
            ILogger<AuthorizationDiagnosticsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("===== AUTHORIZATION DIAGNOSTICS =====");

            _logger.LogInformation(
                "Request Path: {Path}",
                context.Request.Path);

            _logger.LogInformation(
                "User Authenticated: {IsAuthenticated}",
                context.User.Identity?.IsAuthenticated);

            await _next(context);

            _logger.LogInformation(
                "Response Status Code: {StatusCode}",
                context.Response.StatusCode);

            if (context.Response.StatusCode == 403)
            {
                _logger.LogWarning("Authorization Failed: Access Forbidden.");
            }
            else if (context.Response.StatusCode == 401)
            {
                _logger.LogWarning("Authentication Required: Unauthorized.");
            }
            else
            {
                _logger.LogInformation("Request completed.");
            }

            _logger.LogInformation("=====================================");
        }
    }
}
