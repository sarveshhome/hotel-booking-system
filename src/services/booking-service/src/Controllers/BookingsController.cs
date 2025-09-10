using Microsoft.AspNetCore.Mvc;
using MediatR;
using Booking.Service.Application.Features.Bookings.Commands.CreateBooking;

namespace Booking.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateBookingCommand command)
    {
        try
        {
            var bookingId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBooking), new { id = bookingId }, bookingId);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public ActionResult<object> GetBooking(Guid id)
    {
        // TODO: Implement GetBookingById query
        return NotFound();
    }
}
