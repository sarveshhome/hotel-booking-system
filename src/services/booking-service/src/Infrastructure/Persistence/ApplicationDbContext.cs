using Microsoft.EntityFrameworkCore;
using Booking.Service.Domain.Entities;
using Booking.Service.Application.Common.Interfaces;

namespace Booking.Service.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Booking> Bookings => Set<Domain.Entities.Booking>();
    public DbSet<Domain.Entities.Payment> Payments => Set<Domain.Entities.Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Configure Booking entity
        modelBuilder.Entity<Domain.Entities.Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SpecialRequests).HasMaxLength(500);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            
            // Ensure check-out date is after check-in date
            entity.HasCheckConstraint("CK_CheckOutAfterCheckIn", "CheckOutDate > CheckInDate");
        });

        // Configure Payment entity
        modelBuilder.Entity<Domain.Entities.Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PaymentStatus).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TransactionId).IsRequired().HasMaxLength(100);
            
            entity.HasOne(e => e.Booking)
                .WithMany(e => e.Payments)
                .HasForeignKey(e => e.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
