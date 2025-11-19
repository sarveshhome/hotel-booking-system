using MediatR;
using Hotel.Service.Domain.Entities;
using Hotel.Service.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Service.Application.Features.Hotels.Queries.GetHotels;

public record GetHotelsQuery : IRequest<List<HotelDto>>
{
    public string? City { get; init; }
    public string? Country { get; init; }
    public int? MinStarRating { get; init; }
    public bool? IsActive { get; init; }
}

public record HotelDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public int StarRating { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public int RoomCount { get; init; }
}

public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, List<HotelDto>>
{
    private readonly IApplicationDbContext _context;

    public GetHotelsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<HotelDto>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Hotels
            .Include(h => h.Rooms)
            .AsQueryable();

        if (!string.IsNullOrEmpty(request.City))
            query = query.Where(h => h.City.Contains(request.City));

        if (!string.IsNullOrEmpty(request.Country))
            query = query.Where(h => h.Country.Contains(request.Country));

        if (request.MinStarRating.HasValue)
            query = query.Where(h => h.StarRating >= request.MinStarRating.Value);

        if (request.IsActive.HasValue)
            query = query.Where(h => h.IsActive == request.IsActive.Value);

        var hotels = await query
            .Select(h => new HotelDto
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                City = h.City,
                Country = h.Country,
                StarRating = h.StarRating,
                ImageUrl = h.ImageUrl,
                IsActive = h.IsActive,
                RoomCount = h.Rooms.Count
            })
            .ToListAsync(cancellationToken);

        return hotels;
    }
}
