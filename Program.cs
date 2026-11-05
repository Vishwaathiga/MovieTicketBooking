
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.Authorization;
using MovieTicketBooking.Authorization.Requirements;
using MovieTicketBooking.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)),

        ClockSkew = TimeSpan.Zero
    };
});

// Custom Resource-Based Authorization
builder.Services.AddScoped<IAuthorizationHandler, BookingOwnerHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BookingOwner", policy =>
    {
        policy.AddRequirements(new BookingOwnerRequirement());
    });
});

// Repositories and Services
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<ITheatreService, TheatreService>();

builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<IScreenService, ScreenService>();

builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatService, SeatService>();

builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IShowSeatRepository, ShowSeatRepository>();
builder.Services.AddScoped<IShowService, ShowService>();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingSeatRepository, BookingSeatRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// 1. HTTPS Redirection
app.UseHttpsRedirection();

// 2. Routing
app.UseRouting();

// 3. Endpoint Metadata Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<EndpointMetadataMiddleware>();
}

// 4. CORS
app.UseCors("AllowAngular");

// 5. Authentication
app.UseAuthentication();

// 6. Authentication and Authorization Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AuthenticationDiagnosticsMiddleware>();
    app.UseMiddleware<AuthorizationDiagnosticsMiddleware>();
}

// 7. Authorization
app.UseAuthorization();

// 8. Seed Roles and Admin
await RoleSeeder.SeedRolesAndAdminAsync(app.Services);

// 9. Map OpenAPI and Scalar
app.MapOpenApi();
app.MapScalarApiReference();

// 10. Map Controllers
app.MapControllers();

// 10. Run Application
app.Run();
