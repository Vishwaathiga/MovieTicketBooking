
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<Screen> Screens { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<ShowSeat> ShowSeats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingSeat> BookingSeats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Screen configuration
            modelBuilder.Entity<Screen>()
                .HasIndex(s => new { s.TheatreId, s.ScreenName })
                .IsUnique();

            modelBuilder.Entity<Screen>()
                .HasOne(s => s.Theatre)
                .WithMany()
                .HasForeignKey(s => s.TheatreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seat configuration
            modelBuilder.Entity<Seat>()
                .HasIndex(s => new { s.ScreenId, s.RowName, s.SeatNumber })
                .IsUnique();

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Screen)
                .WithMany()
                .HasForeignKey(s => s.ScreenId)
                .OnDelete(DeleteBehavior.Restrict);

            // Show configuration
            modelBuilder.Entity<Show>()
                .HasOne(s => s.Movie)
                .WithMany()
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Show>()
                .HasOne(s => s.Screen)
                .WithMany()
                .HasForeignKey(s => s.ScreenId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Show>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint(
                        "CK_Shows_EndTime",
                        "[EndTime] > [StartTime]");

                    t.HasCheckConstraint(
                        "CK_Shows_TicketPrice",
                        "[TicketPrice] >= 0");
                });

            // ShowSeat configuration
            modelBuilder.Entity<ShowSeat>()
                .HasIndex(ss => new { ss.ShowId, ss.SeatId })
                .IsUnique();

            modelBuilder.Entity<ShowSeat>()
                .HasOne(ss => ss.Show)
                .WithMany(s => s.ShowSeats)
                .HasForeignKey(ss => ss.ShowId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShowSeat>()
                .HasOne(ss => ss.Seat)
                .WithMany()
                .HasForeignKey(ss => ss.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking configuration
            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.BookingReference)
                .IsUnique();

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Show)
                .WithMany()
                .HasForeignKey(b => b.ShowId)
                .OnDelete(DeleteBehavior.Restrict);

            // BookingSeat configuration
            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.Booking)
                .WithMany(b => b.BookingSeats)
                .HasForeignKey(bs => bs.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingSeat>()
                .HasOne(bs => bs.ShowSeat)
                .WithMany()
                .HasForeignKey(bs => bs.ShowSeatId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingSeat>()
           .HasIndex(bs => new { bs.BookingId, bs.ShowSeatId })
           .IsUnique();
        }
    }
}
