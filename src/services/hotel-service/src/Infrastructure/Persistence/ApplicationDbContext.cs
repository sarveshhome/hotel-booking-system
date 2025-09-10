using Microsoft.EntityFrameworkCore;
using Hotel.Service.Domain.Entities;
using Hotel.Service.Application.Common.Interfaces;

namespace Hotel.Service.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Hotel> Hotels => Set<Domain.Entities.Hotel>();
    public DbSet<Domain.Entities.Room> Rooms => Set<Domain.Entities.Room>();
    public DbSet<Domain.Entities.HotelAmenity> HotelAmenities => Set<Domain.Entities.HotelAmenity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Configure Hotel entity
        modelBuilder.Entity<Domain.Entities.Hotel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.StarRating);
            entity.Property(e => e.Latitude).HasPrecision(18, 6);
            entity.Property(e => e.Longitude).HasPrecision(18, 6);
        });

        // Configure Room entity
        modelBuilder.Entity<Domain.Entities.Room>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RoomNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.RoomType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.BasePrice).HasPrecision(18, 2);
            
            entity.HasOne(e => e.Hotel)
                .WithMany(e => e.Rooms)
                .HasForeignKey(e => e.HotelId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure HotelAmenity entity
        modelBuilder.Entity<Domain.Entities.HotelAmenity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            
            entity.HasOne(e => e.Hotel)
                .WithMany(e => e.HotelAmenities)
                .HasForeignKey(e => e.HotelId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        base.OnModelCreating(modelBuilder);
    }
}
