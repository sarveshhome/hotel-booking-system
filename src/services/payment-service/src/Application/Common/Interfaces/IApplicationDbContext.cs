using Microsoft.EntityFrameworkCore;
using Payment.Service.Domain.Entities;

namespace Payment.Service.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.Payment> Payments { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
