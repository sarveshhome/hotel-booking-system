using Microsoft.EntityFrameworkCore;
using Booking.Service.Domain.Entities;

namespace Booking.Service.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Booking> Bookings { get; }
    DbSet<Domain.Entities.Payment> Payments { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
