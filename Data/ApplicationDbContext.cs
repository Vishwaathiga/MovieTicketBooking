using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }

    public DbSet<Theatre> Theatres { get; set; }

    public DbSet<Screen> Screens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Screen>()
            .HasIndex(s => new { s.TheatreId, s.ScreenName })
            .IsUnique();

        modelBuilder.Entity<Screen>()
            .HasOne(s => s.Theatre)
            .WithMany()
            .HasForeignKey(s => s.TheatreId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}