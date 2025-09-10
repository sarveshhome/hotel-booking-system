namespace Pricing.Service.Application.Common.Interfaces;

public interface IPricingEngine
{
    Task<PriceCalculationResult> CalculatePriceAsync(PriceCalculationRequest request, CancellationToken cancellationToken = default);
    Task<List<Domain.Entities.PricingRule>> GetApplicableRulesAsync(Guid hotelId, Guid? roomTypeId, DateTime checkIn, DateTime checkOut, int numberOfGuests, CancellationToken cancellationToken = default);
    Task<decimal> ApplyPricingRulesAsync(decimal basePrice, List<Domain.Entities.PricingRule> rules, PriceCalculationRequest request, CancellationToken cancellationToken = default);
}

public record PriceCalculationRequest
{
    public Guid HotelId { get; init; }
    public Guid? RoomTypeId { get; init; }
    public DateTime CheckInDate { get; init; }
    public DateTime CheckOutDate { get; init; }
    public int NumberOfGuests { get; init; }
    public int NumberOfRooms { get; init; } = 1;
    public string? SpecialEvent { get; init; }
    public string? WeatherCondition { get; init; }
    public decimal? CurrentOccupancyRate { get; init; }
}

public record PriceCalculationResult
{
    public Guid HotelId { get; init; }
    public Guid? RoomTypeId { get; init; }
    public DateTime CheckInDate { get; init; }
    public DateTime CheckOutDate { get; init; }
    public int NumberOfNights { get; init; }
    public decimal BasePrice { get; init; }
    public decimal FinalPrice { get; init; }
    public decimal TotalAmount { get; init; }
    public List<AppliedPricingRule> AppliedRules { get; init; } = new();
    public string Currency { get; init; } = "USD";
    public string CalculationNotes { get; init; } = string.Empty;
}

public record AppliedPricingRule
{
    public Guid RuleId { get; init; }
    public string RuleName { get; init; } = string.Empty;
    public string RuleType { get; init; } = string.Empty;
    public decimal PriceModifier { get; init; }
    public decimal PriceAdjustment { get; init; }
    public string Description { get; init; } = string.Empty;
}
