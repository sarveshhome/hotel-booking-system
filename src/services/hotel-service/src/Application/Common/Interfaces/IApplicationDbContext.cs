using Microsoft.EntityFrameworkCore;
using Hotel.Service.Domain.Entities;

namespace Hotel.Service.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Hotel> Hotels { get; }
    DbSet<Domain.Entities.Room> Rooms { get; }
    DbSet<Domain.Entities.HotelAmenity> HotelAmenities { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
