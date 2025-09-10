namespace Booking.Service.Application.Common.Interfaces;

public interface IHotelService
{
    Task<HotelInfo> GetHotelAsync(Guid hotelId, CancellationToken cancellationToken = default);
    Task<RoomInfo> GetRoomAsync(Guid roomId, CancellationToken cancellationToken = default);
    Task<bool> IsRoomAvailableAsync(Guid roomId, DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken = default);
}

public record HotelInfo
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public int StarRating { get; init; }
}

public record RoomInfo
{
    public Guid Id { get; init; }
    public string RoomNumber { get; init; } = string.Empty;
    public string RoomType { get; init; } = string.Empty;
    public decimal BasePrice { get; init; }
    public int Capacity { get; init; }
}
