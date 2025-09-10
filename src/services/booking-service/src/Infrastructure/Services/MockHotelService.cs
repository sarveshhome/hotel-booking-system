using Booking.Service.Application.Common.Interfaces;

namespace Booking.Service.Infrastructure.Services;

public class MockHotelService : IHotelService
{
    public async Task<HotelInfo> GetHotelAsync(Guid hotelId, CancellationToken cancellationToken = default)
    {
        // Mock hotel data
        return new HotelInfo
        {
            Id = hotelId,
            Name = "Grand Hotel & Spa",
            City = "Miami Beach",
            Country = "USA",
            StarRating = 5
        };
    }

    public async Task<RoomInfo> GetRoomAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        // Mock room data
        return new RoomInfo
        {
            Id = roomId,
            RoomNumber = "101",
            RoomType = "Deluxe Ocean View",
            BasePrice = 299.99m,
            Capacity = 2
        };
    }

    public async Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken = default)
    {
        // Mock availability check - always return true for demo
        // In real implementation, this would check against existing bookings
        return true;
    }
}
