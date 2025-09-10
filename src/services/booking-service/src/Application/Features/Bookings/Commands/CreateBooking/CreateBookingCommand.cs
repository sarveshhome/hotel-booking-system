using MediatR;
using Booking.Service.Domain.Entities;
using Booking.Service.Application.Common.Interfaces;
using Shared.Contracts.Bookings;

namespace Booking.Service.Application.Features.Bookings.Commands.CreateBooking;

public record CreateBookingCommand : IRequest<Guid>
{
    public Guid UserId { get; init; }
    public Guid HotelId { get; init; }
    public Guid RoomId { get; init; }
    public DateTime CheckInDate { get; init; }
    public DateTime CheckOutDate { get; init; }
    public int NumberOfGuests { get; init; }
    public string SpecialRequests { get; init; } = string.Empty;
}

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IEventBus _eventBus;
    private readonly IHotelService _hotelService;

    public CreateBookingCommandHandler(
        IApplicationDbContext context, 
        IEventBus eventBus,
        IHotelService hotelService)
    {
        _context = context;
        _eventBus = eventBus;
        _hotelService = hotelService;
    }

    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        // Validate room availability
        var isAvailable = await _hotelService.IsRoomAvailableAsync(
            request.RoomId, 
            request.CheckInDate, 
            request.CheckOutDate, 
            cancellationToken);

        if (!isAvailable)
        {
            throw new InvalidOperationException("Room is not available for the selected dates");
        }

        // Get room information for pricing
        var roomInfo = await _hotelService.GetRoomAsync(request.RoomId, cancellationToken);
        var hotelInfo = await _hotelService.GetHotelAsync(request.HotelId, cancellationToken);

        // Calculate total amount (simple calculation - can be enhanced with pricing service)
        var numberOfNights = (request.CheckOutDate - request.CheckInDate).Days;
        var totalAmount = roomInfo.BasePrice * numberOfNights;

        var booking = new Domain.Entities.Booking
        {
            UserId = request.UserId,
            HotelId = request.HotelId,
            RoomId = request.RoomId,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            NumberOfGuests = request.NumberOfGuests,
            TotalAmount = totalAmount,
            Status = "Pending",
            SpecialRequests = request.SpecialRequests,
            Created = DateTime.UtcNow,
            CreatedBy = request.UserId.ToString()
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish booking created event
        await _eventBus.PublishAsync(new BookingCreatedEvent
        {
            BookingId = booking.Id,
            UserId = booking.UserId,
            HotelId = booking.HotelId,
            RoomId = booking.RoomId,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status
        }, cancellationToken);

        return booking.Id;
    }
}
