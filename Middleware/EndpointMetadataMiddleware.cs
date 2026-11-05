
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;

namespace MovieTicketBooking.Middleware
{
    public class EndpointMetadataMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<EndpointMetadataMiddleware> _logger;

        public EndpointMetadataMiddleware(
            RequestDelegate next,
            ILogger<EndpointMetadataMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            _logger.LogInformation("===== ENDPOINT METADATA =====");

            if (endpoint != null)
            {
                _logger.LogInformation(
                    "Endpoint Name: {Name}",
                    endpoint.DisplayName);

                var httpMethods = endpoint.Metadata
                    .GetMetadata<HttpMethodMetadata>();

                _logger.LogInformation(
                    "HTTP Methods: {Methods}",
                    httpMethods != null
                        ? string.Join(", ", httpMethods.HttpMethods)
                        : "Not specified");

                var authorizeData = endpoint.Metadata
                    .GetOrderedMetadata<IAuthorizeData>();

                foreach (var authorization in authorizeData)
                {
                    _logger.LogInformation(
                        "Authorization Policy: {Policy}, Roles: {Roles}",
                        authorization.Policy,
                        authorization.Roles);
                }

                var allowAnonymous = endpoint.Metadata
                    .GetMetadata<IAllowAnonymous>();

                _logger.LogInformation(
                    "Allow Anonymous: {AllowAnonymous}",
                    allowAnonymous != null);
            }
            else
            {
                _logger.LogWarning("No endpoint matched this request.");
            }

            _logger.LogInformation("==============================");

            await _next(context);
        }
    }
}
