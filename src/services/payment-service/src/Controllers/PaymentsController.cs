using Microsoft.AspNetCore.Mvc;
using MediatR;
using Payment.Service.Application.Features.Payments.Commands.ProcessPayment;

namespace Payment.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("process")]
    public async Task<ActionResult<Guid>> ProcessPayment(ProcessPaymentCommand command)
    {
        try
        {
            var paymentId = await _mediator.Send(command);
            return Ok(new { paymentId, message = "Payment processed successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public ActionResult<object> GetPayment(Guid id)
    {
        // TODO: Implement GetPaymentById query
        return NotFound();
    }
}
