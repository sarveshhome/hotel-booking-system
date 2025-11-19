using MediatR;
using Hotel.Service.Domain.Entities;
using Hotel.Service.Application.Common.Interfaces;
using Shared.Contracts.Hotels;

namespace Hotel.Service.Application.Features.Hotels.Commands.CreateHotel;

public record CreateHotelCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Website { get; init; } = string.Empty;
    public int StarRating { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public decimal Latitude { get; init; }
    public decimal Longitude { get; init; }
}

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IEventBus _eventBus;

    public CreateHotelCommandHandler(IApplicationDbContext context, IEventBus eventBus)
    {
        _context = context;
        _eventBus = eventBus;
    }

    public async Task<Guid> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = new Hotel.Service.Domain.Entities.Hotel
        {
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            City = request.City,
            State = request.State,
            Country = request.Country,
            PostalCode = request.PostalCode,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Website = request.Website,
            StarRating = request.StarRating,
            ImageUrl = request.ImageUrl,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            IsActive = true,
            Created = DateTime.UtcNow,
            CreatedBy = "System"
        };

        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        await _eventBus.PublishAsync(new HotelCreatedEvent
        {
            HotelId = hotel.Id,
            Name = hotel.Name,
            City = hotel.City,
            Country = hotel.Country,
            StarRating = hotel.StarRating,
            CreatedAt = DateTime.UtcNow
        }, cancellationToken);

        return hotel.Id;
    }
}
