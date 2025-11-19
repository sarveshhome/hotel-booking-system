using Microsoft.EntityFrameworkCore;
using Hotel.Service.Domain.Entities;

namespace Hotel.Service.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Hotel.Service.Domain.Entities.Hotel> Hotels { get; }
    DbSet<Hotel.Service.Domain.Entities.Room> Rooms { get; }
    DbSet<Hotel.Service.Domain.Entities.HotelAmenity> HotelAmenities { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
