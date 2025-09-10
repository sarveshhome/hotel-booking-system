using Microsoft.EntityFrameworkCore;
using Payment.Service.Domain.Entities;
using Payment.Service.Application.Common.Interfaces;

namespace Payment.Service.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Payment> Payments => Set<Domain.Entities.Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Configure Payment entity
        modelBuilder.Entity<Domain.Entities.Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Currency).IsRequired().HasMaxLength(3);
            entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PaymentStatus).IsRequired().HasMaxLength(50);
            entity.Property(e => e.TransactionId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CardLastFourDigits).HasMaxLength(4);
            entity.Property(e => e.CardType).HasMaxLength(20);
            entity.Property(e => e.CardholderName).HasMaxLength(100);
            entity.Property(e => e.FailureReason).HasMaxLength(500);
            entity.Property(e => e.RefundReason).HasMaxLength(500);
            
            // Ensure amount is positive
            entity.HasCheckConstraint("CK_AmountPositive", "Amount > 0");
        });

        base.OnModelCreating(modelBuilder);
    }
}
