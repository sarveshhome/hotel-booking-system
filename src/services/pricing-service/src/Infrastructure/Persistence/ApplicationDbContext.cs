using Microsoft.EntityFrameworkCore;
using Pricing.Service.Domain.Entities;
using Pricing.Service.Application.Common.Interfaces;

namespace Pricing.Service.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.PricingRule> PricingRules => Set<Domain.Entities.PricingRule>();
    public DbSet<Domain.Entities.RoomType> RoomTypes => Set<Domain.Entities.RoomType>();
    public DbSet<Domain.Entities.SeasonalPricing> SeasonalPricing => Set<Domain.Entities.SeasonalPricing>();
    public DbSet<Domain.Entities.DynamicPricing> DynamicPricing => Set<Domain.Entities.DynamicPricing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Configure PricingRule entity
        modelBuilder.Entity<Domain.Entities.PricingRule>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RuleType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.BasePriceModifier).HasPrecision(18, 4);
            entity.Property(e => e.FixedPriceAdjustment).HasPrecision(18, 2);
            entity.Property(e => e.MinimumPrice).HasPrecision(18, 2);
            entity.Property(e => e.MaximumPrice).HasPrecision(18, 2);
            entity.Property(e => e.SpecialEvent).HasMaxLength(100);
            entity.Property(e => e.WeatherCondition).HasMaxLength(50);
            entity.Property(e => e.OccupancyThreshold).HasPrecision(5, 2);
            
            // Indexes for performance
            entity.HasIndex(e => new { e.HotelId, e.RoomTypeId, e.IsActive });
            entity.HasIndex(e => new { e.HotelId, e.StartDate, e.EndDate });
        });

        // Configure RoomType entity
        modelBuilder.Entity<Domain.Entities.RoomType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.BasePrice).HasPrecision(18, 2);
            entity.Property(e => e.Currency).IsRequired().HasMaxLength(3);
            
            entity.HasIndex(e => new { e.HotelId, e.IsActive });
        });

        // Configure SeasonalPricing entity
        modelBuilder.Entity<Domain.Entities.SeasonalPricing>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SeasonName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PriceMultiplier).HasPrecision(18, 4);
            entity.Property(e => e.Description).HasMaxLength(500);
            
            entity.HasIndex(e => new { e.HotelId, e.StartDate, e.EndDate });
        });

        // Configure DynamicPricing entity
        modelBuilder.Entity<Domain.Entities.DynamicPricing>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OccupancyRate).HasPrecision(5, 2);
            entity.Property(e => e.DemandMultiplier).HasPrecision(18, 4);
            entity.Property(e => e.WeatherMultiplier).HasPrecision(18, 4);
            entity.Property(e => e.EventMultiplier).HasPrecision(18, 4);
            entity.Property(e => e.FinalPrice).HasPrecision(18, 2);
            entity.Property(e => e.Notes).HasMaxLength(500);
            
            entity.HasIndex(e => new { e.HotelId, e.Date });
        });

        base.OnModelCreating(modelBuilder);
    }
}
