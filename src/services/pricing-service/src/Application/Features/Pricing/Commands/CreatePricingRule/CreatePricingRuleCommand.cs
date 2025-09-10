using MediatR;
using Pricing.Service.Domain.Entities;
using Pricing.Service.Application.Common.Interfaces;
using Shared.Contracts.Pricing;

namespace Pricing.Service.Application.Features.Pricing.Commands.CreatePricingRule;

public record CreatePricingRuleCommand : IRequest<Guid>
{
    public Guid HotelId { get; init; }
    public Guid? RoomTypeId { get; init; }
    public string RuleType { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public decimal BasePriceModifier { get; init; } = 1.0m;
    public decimal FixedPriceAdjustment { get; init; } = 0.0m;
    public decimal MinimumPrice { get; init; } = 0.0m;
    public decimal MaximumPrice { get; init; } = 0.0m;
    public int? MinimumStay { get; init; }
    public int? MaximumStay { get; init; }
    public int? MinimumOccupancy { get; init; }
    public int? MaximumOccupancy { get; init; }
    public bool Monday { get; init; } = true;
    public bool Tuesday { get; init; } = true;
    public bool Wednesday { get; init; } = true;
    public bool Thursday { get; init; } = true;
    public bool Friday { get; init; } = true;
    public bool Saturday { get; init; } = true;
    public bool Sunday { get; init; } = true;
    public int Priority { get; init; } = 1;
    public string? SpecialEvent { get; init; }
    public string? WeatherCondition { get; init; }
    public decimal? OccupancyThreshold { get; init; }
}

public class CreatePricingRuleCommandHandler : IRequestHandler<CreatePricingRuleCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IEventBus _eventBus;

    public CreatePricingRuleCommandHandler(IApplicationDbContext context, IEventBus eventBus)
    {
        _context = context;
        _eventBus = eventBus;
    }

    public async Task<Guid> Handle(CreatePricingRuleCommand request, CancellationToken cancellationToken)
    {
        var pricingRule = new Domain.Entities.PricingRule
        {
            HotelId = request.HotelId,
            RoomTypeId = request.RoomTypeId,
            RuleType = request.RuleType,
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            BasePriceModifier = request.BasePriceModifier,
            FixedPriceAdjustment = request.FixedPriceAdjustment,
            MinimumPrice = request.MinimumPrice,
            MaximumPrice = request.MaximumPrice,
            MinimumStay = request.MinimumStay,
            MaximumStay = request.MaximumStay,
            MinimumOccupancy = request.MinimumOccupancy,
            MaximumOccupancy = request.MaximumOccupancy,
            Monday = request.Monday,
            Tuesday = request.Tuesday,
            Wednesday = request.Wednesday,
            Thursday = request.Thursday,
            Friday = request.Friday,
            Saturday = request.Saturday,
            Sunday = request.Sunday,
            Priority = request.Priority,
            SpecialEvent = request.SpecialEvent,
            WeatherCondition = request.WeatherCondition,
            OccupancyThreshold = request.OccupancyThreshold,
            IsActive = true,
            Created = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.PricingRules.Add(pricingRule);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        await _eventBus.PublishAsync(new PricingRuleCreatedEvent
        {
            PricingRuleId = pricingRule.Id,
            HotelId = pricingRule.HotelId,
            RoomTypeId = pricingRule.RoomTypeId,
            RuleType = pricingRule.RuleType,
            Name = pricingRule.Name,
            BasePriceModifier = pricingRule.BasePriceModifier,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        return pricingRule.Id;
    }
}
