using Microsoft.EntityFrameworkCore;
using Pricing.Service.Domain.Entities;

namespace Pricing.Service.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Entities.PricingRule> PricingRules { get; }
    DbSet<Domain.Entities.RoomType> RoomTypes { get; }
    DbSet<Domain.Entities.SeasonalPricing> SeasonalPricing { get; }
    DbSet<Domain.Entities.DynamicPricing> DynamicPricing { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
