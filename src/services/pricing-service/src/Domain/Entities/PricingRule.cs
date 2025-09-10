namespace Pricing.Service.Domain.Entities;

public class PricingRule : BaseAuditableEntity
{
    public Guid HotelId { get; set; }
    public Guid? RoomTypeId { get; set; } // Null means applies to all room types
    public string RuleType { get; set; } = string.Empty; // Seasonal, Demand, Special, Base
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Date range for the rule
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    // Pricing modifiers
    public decimal BasePriceModifier { get; set; } = 1.0m; // Multiplier (1.0 = no change, 1.2 = 20% increase)
    public decimal FixedPriceAdjustment { get; set; } = 0.0m; // Fixed amount to add/subtract
    public decimal MinimumPrice { get; set; } = 0.0m;
    public decimal MaximumPrice { get; set; } = 0.0m; // 0 means no maximum
    
    // Conditions
    public int? MinimumStay { get; set; } // Minimum nights required
    public int? MaximumStay { get; set; } // Maximum nights allowed
    public int? MinimumOccupancy { get; set; } // Minimum guests required
    public int? MaximumOccupancy { get; set; } // Maximum guests allowed
    
    // Day of week restrictions
    public bool Monday { get; set; } = true;
    public bool Tuesday { get; set; } = true;
    public bool Wednesday { get; set; } = true;
    public bool Thursday { get; set; } = true;
    public bool Friday { get; set; } = true;
    public bool Saturday { get; set; } = true;
    public bool Sunday { get; set; } = true;
    
    // Priority (higher number = higher priority)
    public int Priority { get; set; } = 1;
    
    // Status
    public bool IsActive { get; set; } = true;
    
    // Advanced conditions
    public string? SpecialEvent { get; set; } // e.g., "Christmas", "New Year"
    public string? WeatherCondition { get; set; } // e.g., "Sunny", "Rainy"
    public decimal? OccupancyThreshold { get; set; } // Hotel occupancy percentage for demand-based pricing
}

public class RoomType : BaseAuditableEntity
{
    public Guid HotelId { get; set; }
    public string Name { get; set; } = string.Empty; // e.g., "Standard", "Deluxe", "Suite"
    public string Description { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public string Currency { get; set; } = "USD";
    public int Capacity { get; set; }
    public bool IsActive { get; set; } = true;
}

public class SeasonalPricing : BaseAuditableEntity
{
    public Guid HotelId { get; set; }
    public Guid? RoomTypeId { get; set; }
    public string SeasonName { get; set; } = string.Empty; // e.g., "Peak", "Off-Peak", "Holiday"
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal PriceMultiplier { get; set; } = 1.0m;
    public string Description { get; set; } = string.Empty;
}

public class DynamicPricing : BaseAuditableEntity
{
    public Guid HotelId { get; set; }
    public Guid? RoomTypeId { get; set; }
    public DateTime Date { get; set; }
    public decimal OccupancyRate { get; set; } // 0.0 to 1.0 (0% to 100%)
    public decimal DemandMultiplier { get; set; } = 1.0m;
    public decimal WeatherMultiplier { get; set; } = 1.0m;
    public decimal EventMultiplier { get; set; } = 1.0m;
    public decimal FinalPrice { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime Created { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? LastModified { get; set; }
    public string LastModifiedBy { get; set; } = string.Empty;
}

public abstract class BaseEntity
{
    public Guid Id { get; set; }
}
