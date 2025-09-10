namespace Shared.Contracts.Pricing;

public class PricingRuleCreatedEvent
{
    public Guid PricingRuleId { get; set; }
    public Guid HotelId { get; set; }
    public Guid? RoomTypeId { get; set; }
    public string RuleType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal BasePriceModifier { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PricingRuleUpdatedEvent
{
    public Guid PricingRuleId { get; set; }
    public Guid HotelId { get; set; }
    public Guid? RoomTypeId { get; set; }
    public string RuleType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal BasePriceModifier { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class PricingRuleDeletedEvent
{
    public Guid PricingRuleId { get; set; }
    public Guid HotelId { get; set; }
    public DateTime DeletedAt { get; set; }
}

public class PriceCalculatedEvent
{
    public Guid HotelId { get; set; }
    public Guid? RoomTypeId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal BasePrice { get; set; }
    public decimal FinalPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime CalculatedAt { get; set; }
}
