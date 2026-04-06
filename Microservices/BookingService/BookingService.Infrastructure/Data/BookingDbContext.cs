using BookingService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Data
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.BookingId);
                entity.Property(b => b.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(b => b.Status).HasMaxLength(20);
                entity.HasIndex(b => b.UserId);
                entity.HasIndex(b => new { b.RoomId, b.StartTime });
            });
        }
    }
}
