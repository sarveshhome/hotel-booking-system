namespace Shared.Contracts.Bookings;
public class BookingCreatedEvent
{
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public Guid HotelId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
}