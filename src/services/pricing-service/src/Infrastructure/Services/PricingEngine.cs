using Pricing.Service.Application.Common.Interfaces;
using Pricing.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Pricing.Service.Infrastructure.Services;

public class PricingEngine : IPricingEngine
{
    private readonly IApplicationDbContext _context;

    public PricingEngine(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PriceCalculationResult> CalculatePriceAsync(PriceCalculationRequest request, CancellationToken cancellationToken = default)
    {
        var numberOfNights = (request.CheckOutDate - request.CheckInDate).Days;
        var basePrice = await GetBasePriceAsync(request.HotelId, request.RoomTypeId, cancellationToken);
        
        // Get applicable pricing rules
        var applicableRules = await GetApplicableRulesAsync(
            request.HotelId, 
            request.RoomTypeId, 
            request.CheckInDate, 
            request.CheckOutDate, 
            request.NumberOfGuests, 
            cancellationToken);

        // Apply pricing rules
        var finalPrice = await ApplyPricingRulesAsync(basePrice, applicableRules, request, cancellationToken);

        // Calculate total amount
        var totalAmount = finalPrice * numberOfNights * request.NumberOfRooms;

        // Create applied rules list
        var appliedRules = applicableRules.Select(rule => new AppliedPricingRule
        {
            RuleId = rule.Id,
            RuleName = rule.Name,
            RuleType = rule.RuleType,
            PriceModifier = rule.BasePriceModifier,
            PriceAdjustment = rule.FixedPriceAdjustment,
            Description = rule.Description
        }).ToList();

        return new PriceCalculationResult
        {
            HotelId = request.HotelId,
            RoomTypeId = request.RoomTypeId,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            NumberOfNights = numberOfNights,
            BasePrice = basePrice,
            FinalPrice = finalPrice,
            TotalAmount = totalAmount,
            AppliedRules = appliedRules,
            CalculationNotes = GenerateCalculationNotes(applicableRules, request)
        };
    }

    public async Task<List<Domain.Entities.PricingRule>> GetApplicableRulesAsync(
        Guid hotelId, 
        Guid? roomTypeId, 
        DateTime checkIn, 
        DateTime checkOut, 
        int numberOfGuests, 
        CancellationToken cancellationToken = default)
    {
        var rules = await _context.PricingRules
            .Where(pr => pr.HotelId == hotelId && pr.IsActive)
            .Where(pr => !pr.RoomTypeId.HasValue || pr.RoomTypeId == roomTypeId)
            .Where(pr => !pr.StartDate.HasValue || pr.StartDate <= checkIn)
            .Where(pr => !pr.EndDate.HasValue || pr.EndDate >= checkOut)
            .Where(pr => !pr.MinimumStay.HasValue || (checkOut - checkIn).Days >= pr.MinimumStay.Value)
            .Where(pr => !pr.MaximumStay.HasValue || (checkOut - checkIn).Days <= pr.MaximumStay.Value)
            .Where(pr => !pr.MinimumOccupancy.HasValue || numberOfGuests >= pr.MinimumOccupancy.Value)
            .Where(pr => !pr.MaximumOccupancy.HasValue || numberOfGuests <= pr.MaximumOccupancy.Value)
            .OrderByDescending(pr => pr.Priority)
            .ToListAsync(cancellationToken);

        // Filter by day of week
        var applicableRules = new List<PricingRule>();
        foreach (var rule in rules)
        {
            if (IsRuleApplicableForDates(rule, checkIn, checkOut))
            {
                applicableRules.Add(rule);
            }
        }

        return applicableRules;
    }

    public async Task<decimal> ApplyPricingRulesAsync(decimal basePrice, List<Domain.Entities.PricingRule> rules, PriceCalculationRequest request, CancellationToken cancellationToken = default)
    {
        var finalPrice = basePrice;

        foreach (var rule in rules.OrderByDescending(r => r.Priority))
        {
            // Apply base price modifier
            finalPrice *= rule.BasePriceModifier;

            // Apply fixed price adjustment
            finalPrice += rule.FixedPriceAdjustment;

            // Apply special event multiplier
            if (!string.IsNullOrEmpty(rule.SpecialEvent) && 
                !string.IsNullOrEmpty(request.SpecialEvent) &&
                rule.SpecialEvent.Equals(request.SpecialEvent, StringComparison.OrdinalIgnoreCase))
            {
                finalPrice *= 1.1m; // 10% increase for special events
            }

            // Apply weather condition multiplier
            if (!string.IsNullOrEmpty(rule.WeatherCondition) && 
                !string.IsNullOrEmpty(request.WeatherCondition) &&
                rule.WeatherCondition.Equals(request.WeatherCondition, StringComparison.OrdinalIgnoreCase))
            {
                finalPrice *= 1.05m; // 5% increase for specific weather
            }

            // Apply occupancy-based pricing
            if (rule.OccupancyThreshold.HasValue && request.CurrentOccupancyRate.HasValue)
            {
                if (request.CurrentOccupancyRate.Value > rule.OccupancyThreshold.Value)
                {
                    finalPrice *= 1.15m; // 15% increase for high occupancy
                }
                else if (request.CurrentOccupancyRate.Value < 0.3m)
                {
                    finalPrice *= 0.9m; // 10% discount for low occupancy
                }
            }

            // Apply minimum/maximum price constraints
            if (rule.MinimumPrice > 0 && finalPrice < rule.MinimumPrice)
            {
                finalPrice = rule.MinimumPrice;
            }

            if (rule.MaximumPrice > 0 && finalPrice > rule.MaximumPrice)
            {
                finalPrice = rule.MaximumPrice;
            }
        }

        return Math.Round(finalPrice, 2);
    }

    private async Task<decimal> GetBasePriceAsync(Guid hotelId, Guid? roomTypeId, CancellationToken cancellationToken)
    {
        if (roomTypeId.HasValue)
        {
            var roomType = await _context.RoomTypes
                .FirstOrDefaultAsync(rt => rt.Id == roomTypeId.Value && rt.HotelId == hotelId, cancellationToken);
            
            if (roomType != null)
                return roomType.BasePrice;
        }

        // Default base price if no room type specified
        return 100.00m;
    }

    private bool IsRuleApplicableForDates(PricingRule rule, DateTime checkIn, DateTime checkOut)
    {
        var currentDate = checkIn.Date;
        while (currentDate < checkOut.Date)
        {
            var dayOfWeek = currentDate.DayOfWeek;
            bool isDayAllowed = dayOfWeek switch
            {
                DayOfWeek.Monday => rule.Monday,
                DayOfWeek.Tuesday => rule.Tuesday,
                DayOfWeek.Wednesday => rule.Wednesday,
                DayOfWeek.Thursday => rule.Thursday,
                DayOfWeek.Friday => rule.Friday,
                DayOfWeek.Saturday => rule.Saturday,
                DayOfWeek.Sunday => rule.Sunday,
                _ => true
            };

            if (!isDayAllowed)
                return false;

            currentDate = currentDate.AddDays(1);
        }

        return true;
    }

    private string GenerateCalculationNotes(List<PricingRule> rules, PriceCalculationRequest request)
    {
        var notes = new List<string>();

        if (rules.Any(r => r.RuleType == "Seasonal"))
            notes.Add("Seasonal pricing applied");

        if (rules.Any(r => r.RuleType == "Demand"))
            notes.Add("Demand-based pricing applied");

        if (rules.Any(r => !string.IsNullOrEmpty(r.SpecialEvent)))
            notes.Add($"Special event pricing: {string.Join(", ", rules.Where(r => !string.IsNullOrEmpty(r.SpecialEvent)).Select(r => r.SpecialEvent))}");

        if (request.CurrentOccupancyRate.HasValue)
            notes.Add($"Occupancy rate: {request.CurrentOccupancyRate.Value:P0}");

        return string.Join("; ", notes);
    }
}
