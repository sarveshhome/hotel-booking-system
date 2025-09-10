using MediatR;
using Microsoft.EntityFrameworkCore;
using Pricing.Service.Application.Common.Interfaces;
using Pricing.Service.Domain.Entities;

namespace Pricing.Service.Application.Features.Pricing.Queries.CalculatePrice;

public record CalculatePriceQuery : IRequest<PriceCalculationResult>
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

public class CalculatePriceQueryHandler : IRequestHandler<CalculatePriceQuery, PriceCalculationResult>
{
    private readonly IPricingEngine _pricingEngine;
    private readonly IApplicationDbContext _context;

    public CalculatePriceQueryHandler(IPricingEngine pricingEngine, IApplicationDbContext context)
    {
        _pricingEngine = pricingEngine;
        _context = context;
    }

    public async Task<PriceCalculationResult> Handle(CalculatePriceQuery request, CancellationToken cancellationToken)
    {
        // Get base price from room type or default
        var basePrice = await GetBasePriceAsync(request.HotelId, request.RoomTypeId, cancellationToken);
        
        // Create pricing request
        var pricingRequest = new PriceCalculationRequest
        {
            HotelId = request.HotelId,
            RoomTypeId = request.RoomTypeId,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            NumberOfGuests = request.NumberOfGuests,
            NumberOfRooms = request.NumberOfRooms,
            SpecialEvent = request.SpecialEvent,
            WeatherCondition = request.WeatherCondition,
            CurrentOccupancyRate = request.CurrentOccupancyRate
        };

        // Calculate final price using pricing engine
        var result = await _pricingEngine.CalculatePriceAsync(pricingRequest, cancellationToken);

        return result;
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
        return 100.00m; // This should come from hotel service in real implementation
    }
}
