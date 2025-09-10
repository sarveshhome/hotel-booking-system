using Microsoft.AspNetCore.Mvc;
using MediatR;
using Hotel.Service.Application.Features.Hotels.Commands.CreateHotel;
using Hotel.Service.Application.Features.Hotels.Queries.GetHotels;

namespace Hotel.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public HotelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateHotelCommand command)
    {
        var hotelId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetHotels), new { }, hotelId);
    }

    [HttpGet]
    public async Task<ActionResult<List<HotelDto>>> GetHotels(
        [FromQuery] string? city,
        [FromQuery] string? country,
        [FromQuery] int? minStarRating,
        [FromQuery] bool? isActive)
    {
        var query = new GetHotelsQuery
        {
            City = city,
            Country = country,
            MinStarRating = minStarRating,
            IsActive = isActive
        };

        var hotels = await _mediator.Send(query);
        return Ok(hotels);
    }

    [HttpGet("{id}")]
    public ActionResult<HotelDto> GetHotel(Guid id)
    {
        // TODO: Implement GetHotelById query
        return NotFound();
    }
}
